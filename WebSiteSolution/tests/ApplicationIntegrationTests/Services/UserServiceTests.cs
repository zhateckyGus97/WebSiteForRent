using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationIntegrationTests.Services
{
    public class UserServiceTests : IClassFixture<TestingFixture>
    {
        private TestingFixture _fixture;
        public UserServiceTests(TestingFixture fixture)
        {
            _fixture = fixture;
        }

        public async Task Test()
        {

        }
    }
}
