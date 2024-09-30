namespace Webshop.Review.API.Utilities
{
    /// <summary>
    /// This is a simple helper class that returns a unique identifier for the instance of the service.
    /// It is then registered as a singleton, meaning that the same instance is used throughout the lifetime of the application.
    /// This way I do not have to implement the singleton pattern myself.
    /// It will first try to take a value from a environment variable, if it is not present it will generate a new Guid.
    /// </summary>
    public class InstanceHelper
    {
        private readonly string instanceId;
        public InstanceHelper()
        {
            //get the instance id from the environment variable
            var instanceIdFromEnv = Environment.GetEnvironmentVariable("INSTANCE_ID");
            // If the environment variable is not set, generate a new Guid
            this.instanceId = !string.IsNullOrEmpty(instanceIdFromEnv) ? instanceIdFromEnv : Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Returns the instanceId
        /// </summary>
        /// <returns></returns>
        public string GetInstanceId()
        {
            return this.instanceId;
        }
    }
}
