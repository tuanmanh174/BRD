using BRD.DataAccess;
using BRD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Countries = BRD.DataAccess.Countries;

namespace BRD.Service
{
    public class CountryService : ICountryRepository
    {

        private Context _context;
        public CountryService(Context context)
        {
            _context = context;
        }

        public IList<Countries> GetAll()
        {
            IList<Countries> list = new List<Countries>();
            try
            {
                var lst = _context.COUNTRIES.ToList();
                foreach (var c in lst)
                {
                    var ab = new Countries();
                    ab.COUNTRY_NAME = c.COUNTRY_NAME;
                    ab.REGION_ID = c.REGION_ID;
                    ab.COUNTRY_ID = c.COUNTRY_ID;
                    list.Add(ab);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return list;
        }
        public Countries GetById(string account)
        {
            var c = _context.COUNTRIES.Where(x => x.COUNTRY_NAME == account).FirstOrDefault();
            var ab = new Countries();
            ab.COUNTRY_NAME = c.COUNTRY_NAME;
            ab.REGION_ID = c.REGION_ID;
            ab.COUNTRY_ID = c.COUNTRY_ID;
            return ab;
        }
       

       
    }
}
