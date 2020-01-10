function Get-MainProjects {
    Param ([string]$relativePathToSrc)

    $mainProjects = "${relativePathToSrc}core\main\rapidcore.csproj", 
        "${relativePathToSrc}google-cloud\main\rapidcore.google-cloud.csproj",
        "${relativePathToSrc}mongo\main\rapidcore.mongo.csproj", 
        "${relativePathToSrc}postgresql\main\rapidcore.postgresql.csproj", 
        "${relativePathToSrc}redis\main\rapidcore.redis.csproj", 
        "${relativePathToSrc}xunit\main\rapidcore.xunit.csproj", 
        "${relativePathToSrc}sqlserver\main\rapidcore.sqlserver.csproj"

    return $mainProjects
}
