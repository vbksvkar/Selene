namespace Clients.Tests
{
    public static class ExceptionAsserts
    {
        public static void Throws<T>(Action func) where T : Exception
        {
            var exceptionThrown = false;
            try 
            {
                func.Invoke();
            }
            catch (T)
            {
                exceptionThrown = true;
            }
            
            if (!exceptionThrown)
            {
                throw new Exception($"Expected exception of type {typeof(T)} but no exception was thrown.");
            }
        }
    }
}