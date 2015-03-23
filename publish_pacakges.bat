move LM.Core.Domain\Releases\*.nupkg LM.Core.Domain\Releases\old
nuget pack LM.Core.Domain\LM.Core.Domain.csproj -Prop Configuration=Release -OutputDirectory LM.Core.Domain\Releases
nuget push -Source http://nuget.smv.br -ApiKey 648CA9AC80FE4CAFA2E581E0FEADCC26 LM.Core.Domain\Releases\*.nupkg

move LM.Core.Application\Releases\*.nupkg LM.Core.Application\Releases\old
nuget pack LM.Core.Application\LM.Core.Application.csproj -Prop Configuration=Release -OutputDirectory LM.Core.Application\Releases
nuget push -Source http://nuget.smv.br -ApiKey 648CA9AC80FE4CAFA2E581E0FEADCC26 LM.Core.Application\Releases\*.nupkg

move LM.Core.Repository\Releases\*.nupkg LM.Core.Repository\Releases\old
nuget pack LM.Core.Repository\LM.Core.Repository.csproj -Prop Configuration=Release -OutputDirectory LM.Core.Repository\Releases
nuget push -Source http://nuget.smv.br -ApiKey 648CA9AC80FE4CAFA2E581E0FEADCC26 LM.Core.Repository\Releases\*.nupkg

move LM.Core.RepositorioEF\Releases\*.nupkg LM.Core.RepositorioEF\Releases\old
nuget pack LM.Core.RepositorioEF\LM.Core.RepositorioEF.csproj -Prop Configuration=Release -OutputDirectory LM.Core.RepositorioEF\Releases
nuget push -Source http://nuget.smv.br -ApiKey 648CA9AC80FE4CAFA2E581E0FEADCC26 LM.Core.RepositorioEF\Releases\*.nupkg