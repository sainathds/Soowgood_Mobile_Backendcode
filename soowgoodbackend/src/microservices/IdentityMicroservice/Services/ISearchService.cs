using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels.Search;
using System.Collections.Generic;

namespace IdentityMicroservice.Services
{
    public interface ISearchService
    {
        List<Search> GlobalSearch(SearchParameterViewModel model);
        List<Search> GetProviderInfo(SearchParameterViewModel model);
    }
}