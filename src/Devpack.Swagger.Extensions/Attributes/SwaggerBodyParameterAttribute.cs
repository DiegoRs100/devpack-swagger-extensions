namespace Devpack.Swagger.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerBodyParameterAttribute : Attribute
    {
        public string Description { get; private set; }

        public SwaggerBodyParameterAttribute(string description)
        {
            Description = description;
        }
    }
}