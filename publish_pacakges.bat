move LM.Core.Domain\NugetReleases\*.nupkg LM.Core.Domain\NugetReleases\old
nuget pack LM.Core.Domain\LM.Core.Domain.csproj -Prop Configuration=Release -OutputDirectory LM.Core.Domain\NugetReleases
nuget push -Source http://nuget.smv.br -ApiKey 648CA9AC80FE4CAFA2E581E0FEADCC26 LM.Core.Domain\NugetReleases\*.nupkg

move LM.Core.Application\NugetReleases\*.nupkg LM.Core.Application\NugetReleases\old
nuget pack LM.Core.Application\LM.Core.Application.csproj -Prop Configuration=Release -OutputDirectory LM.Core.Application\NugetReleases
nuget push -Source http://nuget.smv.br -ApiKey 648CA9AC80FE4CAFA2E581E0FEADCC26 LM.Core.Application\NugetReleases\*.nupkg

move LM.Core.RepositorioEF\NugetReleases\*.nupkg LM.Core.RepositorioEF\NugetReleases\old
nuget pack LM.Core.RepositorioEF\LM.Core.RepositorioEF.csproj -Prop Configuration=Release -OutputDirectory LM.Core.RepositorioEF\NugetReleases
nuget push -Source http://nuget.smv.br -ApiKey 648CA9AC80FE4CAFA2E581E0FEADCC26 LM.Core.RepositorioEF\NugetReleases\*.nupkg