```sh
dotnet build -c release --no-incremental && dotnet pack -p:RepositoryCommit="$(git rev-parse head)" -p:Version=1.0.0-alpha2
```