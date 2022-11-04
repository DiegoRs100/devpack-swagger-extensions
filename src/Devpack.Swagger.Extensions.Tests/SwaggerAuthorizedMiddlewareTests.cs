using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Devpack.Swagger.Extensions.Tests
{
    public class SwaggerAuthorizedMiddlewareTests
    {
        public readonly Mock<RequestDelegate> _requestDelegateMock;
        public readonly Mock<HttpContext> _httpContextMock;

        public SwaggerAuthorizedMiddlewareTests()
        {
            _requestDelegateMock = new Mock<RequestDelegate>();
            _httpContextMock = new Mock<HttpContext>();

            _httpContextMock.Setup(m => m.Response).Returns(new Mock<HttpResponse>().Object);
        }

        [Fact(DisplayName = "Deve setar o status 401 quando um endpoint do swagger for chamado e o usuário não estiver autenticado.")]
        [Trait("Category", "Middlewares")]
        public async Task Invoke_WhenUserUnauthenticated()
        {
            _httpContextMock.Setup(m => m.Request.Path).Returns("/swagger/test");
            _httpContextMock.Setup(m => m.User.Identity!.IsAuthenticated).Returns(false);

            var middleware = new SwaggerAuthorizedMiddleware(_requestDelegateMock.Object);
            await middleware.Invoke(_httpContextMock.Object);

            _httpContextMock.VerifySet(m => m.Response.StatusCode = StatusCodes.Status401Unauthorized, Times.Once);
            _requestDelegateMock.Verify(m => m.Invoke(It.IsAny<HttpContext>()), Times.Never);
        }

        [Fact(DisplayName = "Deve manter os status inalterado quando um endpoint do swagger for chamado e o usuário estiver autenticado.")]
        [Trait("Category", "Middlewares")]
        public async Task Invoke_WhenUserIAthenticated()
        {
            _httpContextMock.Setup(m => m.Request.Path).Returns("/swagger/test");
            _httpContextMock.Setup(m => m.User.Identity!.IsAuthenticated).Returns(true);

            var middleware = new SwaggerAuthorizedMiddleware(_requestDelegateMock.Object);
            await middleware.Invoke(_httpContextMock.Object);

            _httpContextMock.VerifySet(m => m.Response.StatusCode = It.IsAny<int>(), Times.Never);
            _requestDelegateMock.Verify(m => m.Invoke(_httpContextMock.Object), Times.Once);
        }

        [Fact(DisplayName = "Deve manter os status inalterado quando um endpoint que não seja do swagger for chamado.")]
        [Trait("Category", "Middlewares")]
        public async Task Invoke_WhenEndpointIsNotSwagger()
        {
            _httpContextMock.Setup(m => m.Request.Path).Returns("/api/test");
            _httpContextMock.Setup(m => m.User.Identity!.IsAuthenticated).Returns(true);

            var middleware = new SwaggerAuthorizedMiddleware(_requestDelegateMock.Object);
            await middleware.Invoke(_httpContextMock.Object);

            _httpContextMock.VerifySet(m => m.Response.StatusCode = It.IsAny<int>(), Times.Never);
            _requestDelegateMock.Verify(m => m.Invoke(_httpContextMock.Object), Times.Once);
        }
    }
}