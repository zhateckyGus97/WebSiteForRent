using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ApplicationUnitTests.Services
{
    public class AttachmentServiceTests
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAttachmentService _attachmentService;
        private Mock<IAttachmentRepository> _attachmentRepositoryMock;
        private Mock<IFileStorageService> _fileStorageServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Faker _faker;

        public AttachmentServiceTests()
        {
            _faker = new Faker();

            _attachmentRepositoryMock = new Mock<IAttachmentRepository>();
            _attachmentRepository = _attachmentRepositoryMock.Object;

            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _fileStorageService = _fileStorageServiceMock.Object;

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessor = _httpContextAccessorMock.Object;

            _attachmentService = new AttachmentService(_attachmentRepository, _fileStorageService, _httpContextAccessor);
        }

        [Fact]
        public async Task UploadAsync_ValidFile_ReturnsAttachmentWithCorrectProperties()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var category = "test-category";
            var originalFileName = "test-image.jpg";
            var contentType = "image/jpeg";
            var fileSize = 1024;
            var storedPath = $"/storage/{category}/generated-filename.jpg";

            fileMock.Setup(f => f.FileName).Returns(originalFileName);
            fileMock.Setup(f => f.ContentType).Returns(contentType);
            fileMock.Setup(f => f.Length).Returns(fileSize);

            _fileStorageServiceMock.Setup(x => x.SaveFile(
                It.IsAny<IFormFile>(),
                category,
                It.IsAny<string>()
            )).ReturnsAsync(storedPath);

            // Act
            var result = await _attachmentService.UploadAsync(fileMock.Object, category);

            // Assert
            result.Should().NotBeNull();
            result.FileName.Should().Be(originalFileName);
            result.StoredPath.Should().Be(storedPath);
            result.ContentType.Should().Be(contentType);
            result.Size.Should().Be(fileSize);
            result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

            _fileStorageServiceMock.Verify(x => x.SaveFile(
                fileMock.Object,
                category,
                It.Is<string>(f => f.EndsWith(".jpg"))),
                Times.Once);

            _attachmentRepositoryMock.Verify(x => x.Save(
                It.Is<Attachment>(a =>
                    a.FileName == originalFileName &&
                    a.StoredPath == storedPath &&
                    a.ContentType == contentType &&
                    a.Size == fileSize
                )),
                Times.Once);
        }

        [Fact]
        public async Task GetFileContentAsync_ExistingFile_ReturnsContent()
        {
            // Arrange
            var id = 1;
            var attachment = new Attachment { Id = id, FileName = "noname", ContentType = ".jpg", StoredPath = "/path/to/file" };
            var fileContent = new byte[] { 1, 2, 3 };

            _attachmentRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(attachment);
            _fileStorageServiceMock.Setup(x => x.FileExists(attachment.StoredPath)).Returns(true);
            _fileStorageServiceMock.Setup(x => x.ReadFile(attachment.StoredPath)).ReturnsAsync(fileContent);

            // Act
            var result = await _attachmentService.GetFileContentAsync(id);

            // Assert
            result.Should().Equal(fileContent);
        }

        [Fact]
        public async Task GetFileContentAsync_NonExistingFile_ThrowsException()
        {
            // Arrange
            var id = 1;

            _attachmentRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync((Attachment)null);

            // Act & Assert
            await _attachmentService.Invoking(x => x.GetFileContentAsync(id))
                .Should().ThrowAsync<NotFoundApplicationException>()
                .WithMessage("Attachment not found");
        }

        [Fact]
        public async Task DeleteAsync_ExistingFile_DeletesFile()
        {
            // Arrange
            var id = 1;
            var attachment = new Attachment { Id = id, FileName = "noname", ContentType = ".jpg", StoredPath = "/path/to/file" };

            _attachmentRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(attachment);
            _fileStorageServiceMock.Setup(x => x.DeleteFile(attachment.StoredPath));
            _attachmentRepositoryMock.Setup(x => x.Delete(id)).Returns(Task.CompletedTask);

            // Act
            await _attachmentService.DeleteAsync(id);

            // Assert
            _fileStorageServiceMock.Verify(x => x.DeleteFile(attachment.StoredPath), Times.Once);
            _attachmentRepositoryMock.Verify(x => x.Delete(id), Times.Once);
        }

        [Fact]
        public async Task GetPublicLinkAsync_ExistingFile_ReturnsLink()
        {
            // Arrange
            var id = 1;
            var attachment = new Attachment { Id = id, FileName = "noname", ContentType = ".jpg", StoredPath = "/path/to/file" };
            var expectedLink = "http://localhost/api/attachments/1/download";

            var context = new DefaultHttpContext();
            context.Request.Scheme = "http";
            context.Request.Host = new HostString("localhost");

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);
            _attachmentRepositoryMock.Setup(x => x.Get(id)).ReturnsAsync(attachment);

            // Act
            var result = await _attachmentService.GetPublicLinkAsync(id);

            // Assert
            result.Should().Be(expectedLink);
        }
    }
}
