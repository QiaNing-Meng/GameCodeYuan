namespace Protocol
{
    public class TS_Singleton<T> where T : new ()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                return Equals(instance, default(T)) ? (instance = new T()) : instance;
            }
        }
    }
}
