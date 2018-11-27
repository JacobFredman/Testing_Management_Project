namespace BL
{
    public static class FactoryBL
    {
        private static BlImp _bl = null;

        public static BlImp GetObject
        {
            get
            {
                if (_bl == null)
                    _bl = new BlImp();
                return _bl;
            }
        }

    }
}
