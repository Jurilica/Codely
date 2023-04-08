using Codely.Core.Gateways.Contracts;
using Refit;

namespace Codely.Core.Gateways;

public interface ICodeTranslationApi
{
    [Post("api/v2/piston/execute")]
    Task<TranslateCodeResponse> TranslateCode([Body] TranslateCodeRequest translateCodeRequest);
}