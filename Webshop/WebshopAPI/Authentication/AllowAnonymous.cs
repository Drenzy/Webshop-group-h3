namespace WebshopAPI.Authentication
{

        [AttributeUsage(AttributeTargets.Method)]
    // making the AllowAnonymousAttribute an attribute
    public class AllowAnonymousAttribute : Attribute { }
}
