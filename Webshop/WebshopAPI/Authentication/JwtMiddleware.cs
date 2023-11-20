namespace WebshopAPI.Authentication
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILoginRepository loginRepository, IJwtUtils jwtUtils)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            int? userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                var user = await loginRepository.FindByIdAsync(userId.Value);
                context.Items["User"] = new UserResponse 
                {
                    Id= userId.Value,
                    Role = user.Role
                };
            }
            await _next(context);
        }
    }
}
