using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Duende.IdentityModel.Client;

using Microsoft.Identity.Client;

using Storm.TechTask.Api.Endpoints.Project;
using Storm.TechTask.Api.IntegrationTests.Utilities;
using Storm.TechTask.SharedKernel.Authorization;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.Api.IntegrationTests.Endpoints.Projects
{
    [Collection("Sequential")]
    public class GetByIdEndpointTests : BaseEndpointFixture
    {
        public GetByIdEndpointTests(ITestOutputHelper output, CustomWebApplicationFactory factory) : base(output, factory)
        {
        }
        /*
        Tests:
        return a project using an existing ID
        returns a 'not found' if using an ID that doesn't exist
        test for unauthenticated user
        test for authenitcated user (with token) but not an admin (test for unauthorised user)
            */

        [Fact]
        public async Task ReturnsProjectUsingAnExistingID()
        {
            // arrange
            var project = await NewProject().BuildAndPersist(); // builds object and saves to db (we're using it's ID)
            // admin role needed to access endpoint
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(AppRole.ProjectAdmin));

            // act
            var response = await this.HttpClient.GetAsync($"/Projects/{project.Id}");

            // assert
            // checks json response matches expected obj
            await response.ShouldBeSuccess().WithObjectPayload(new ProjectDto(project.Id, project.Name));
        }

        [Fact]
        public async Task ReturnsNotFoundForNonExistentId()
        {
            // Arrange
            var nonExistentId = 999999;
            HttpClient.SetBearerToken(await TokenIssuer.GetNewToken(AppRole.ProjectAdmin));

            // Act
            var response = await HttpClient.GetAsync($"/Projects/{nonExistentId}");

            // Assert
            response.ShouldBeNotFound();
        }


        [Fact]
        public async Task ShouldBeUnauthorized()
        {
            // Arrange
            var project = await NewProject().BuildAndPersist();
            // the token provides authentication, so no token = no authentication, regardless of user role

            // Act
            var response = await HttpClient.GetAsync($"/Projects/{project.Id}");

            // Assert
            response.ShouldBeNotAuthenticated();
        }

        [Theory]
        [AllRolesExcept(AppRole.ProjectAdmin)]
        public async Task ShouldBeForbiddenForOtherRoles(AppRole role)
            // the GetById endpoint is restricted to admin roles
        {
            // Arrange
            var project = await NewProject().BuildAndPersist();
            HttpClient.SetBearerToken(await TokenIssuer.GetNewToken(role));
            // this access token is not for admins

            // Act
            var response = await HttpClient.GetAsync($"/Projects/{project.Id}");

            // Assert
            response.ShouldBeNotAuthorised();
        }
    }
}
