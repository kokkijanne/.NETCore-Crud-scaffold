using Microsoft.Extensions.Options;
using Todo.Api.Middleware;
using Todo.Api.Middleware.Services;

namespace Todo.Api.Middleware.Services
{
    public class PinService : IPinService
    {
        private readonly PinOptions options;

        public PinService(IOptions<PinOptions> options)
        {
            this.options = options.Value;
        }

        public bool IsCorrect(string pin) =>
            !string.IsNullOrWhiteSpace(this.options.PinCode) &&
            !string.IsNullOrWhiteSpace(pin) &&
            string.Equals(this.options.PinCode, pin);
    }
}
