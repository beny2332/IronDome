namespace IronDome.DAL
{
   
    public class Data
    {
        string ConnectionString = "server = LAPTOP-ARI; initial catalog = iron_dome_db; user id = sa; password = 211488770; TrustServerCertificate=Yes";

        static Data? _data;

        private Data()
        {
            DataLayer = new DataLayer(ConnectionString);
        }

        private DataLayer DataLayer;

        public static DataLayer Get
        {
            get
            {
                if (_data == null) 
                {
                    _data = new Data(); 
                }

                return _data.DataLayer;
            }
        }
    }
}
