using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData.Manipulator;
using IdentityMicroservice.ViewModels.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Services
{
    public class SearchService : ISearchService
    {
        public List<Search> GlobalSearch(SearchParameterViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@SearchKeyword", SqlDbType.NVarChar, model.SearchKeyword));
            _params.Add(new SqlParam("@Location", SqlDbType.NVarChar, model.Location));
            _params.Add(new SqlParam("@ServiceType", SqlDbType.NVarChar, model.ServiceType));
            _params.Add(new SqlParam("@AppointmentType", SqlDbType.NVarChar, model.AppointmentType));

            _params.Add(new SqlParam("@ProviderType", SqlDbType.NVarChar, model.ProviderType));
            _params.Add(new SqlParam("@ProviderSpeciality", SqlDbType.NVarChar, model.ProviderSpeciality));
            _params.Add(new SqlParam("@ConsultationFees", SqlDbType.Int, model.ConsultationFees));

            _params.Add(new SqlParam("@Availability", SqlDbType.NVarChar, model.Availability));
            _params.Add(new SqlParam("@Gender", SqlDbType.NVarChar, model.Gender));
            _params.Add(new SqlParam("@DayStartingTime", SqlDbType.DateTime, model.DayStartingTime));
            _params.Add(new SqlParam("@DayEndTime", SqlDbType.DateTime, model.DayEndTime));

            var items = Executor.ExecuteStoredProcedure<Search>("GetSearchResult", _params);
            return items;
        }

        public List<Search> GetProviderInfo(SearchParameterViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@UserRole", SqlDbType.NVarChar, model.UserRole));

            var items = Executor.ExecuteStoredProcedure<Search>("GetProviderInfo", _params);
            return items;
        }
    }
}
