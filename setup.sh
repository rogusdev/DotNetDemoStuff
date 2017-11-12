mkdir DemoStuff
cd DemoStuff

wget https://raw.githubusercontent.com/github/gitignore/master/VisualStudio.gitignore -O .gitignore

dotnet new console -f netcoreapp2.0 -n DemoStuff
dotnet new xunit -f netcoreapp2.0 -n DemoStuff.Tests
dotnet add ./DemoStuff.Tests package xunit --version 2.3.1  # update the version
dotnet add ./DemoStuff.Tests package FakeItEasy --version 4.2.0
dotnet add ./DemoStuff.Tests reference ./DemoStuff/DemoStuff.csproj

# because xunit requires a tool as well, and because dotnet does not add this automatically...
#  https://github.com/NuGet/Home/issues/4901
#  https://xunit.github.io/docs/getting-started-dotnet-core
sed -i '/PackageReference Include="Microsoft.NET.Test.Sdk"/ d' DemoStuff.Tests/DemoStuff.Tests.csproj
sed -i 's/<PackageReference Include="xunit.runner.visualstudio" Version="2.2.0" \/>/<DotNetCliToolReference Include="dotnet-xunit" Version="2.3.1" \/>/' DemoStuff.Tests/DemoStuff.Tests.csproj

# unfortunately tho, can't use this for the sln, can only use dotnet xunit in the project
#  https://github.com/dotnet/cli/issues/6357

# https://github.com/tonerdo/dotnet-env
dotnet add ./DemoStuff package DotNetEnv --version 1.1.0
# http://www.npgsql.org/doc/index.html
dotnet add ./DemoStuff package Npgsql --version 3.2.5
# https://github.com/StackExchange/Dapper
dotnet add ./DemoStuff package Dapper --version 1.50.2
# https://stackexchange.github.io/StackExchange.Redis/
# note that ServiceStack.Redis is a commercial product with limitations on the free version
# https://stackoverflow.com/questions/8902674/manually-map-column-names-with-class-properties/34536863#34536863
dotnet add ./DemoStuff package StackExchange.Redis --version 1.2.6
# https://github.com/confluentinc/confluent-kafka-dotnet
dotnet add ./DemoStuff package Confluent.Kafka --version 0.11.2
# https://www.quartz-scheduler.net/documentation/quartz-3.x/quick-start.html
dotnet add ./DemoStuff package Quartz --version 3.0.0-beta1

dotnet new sln -n DemoStuff
dotnet sln add DemoStuff/DemoStuff.csproj
dotnet sln add DemoStuff.Tests/DemoStuff.Tests.csproj

#cd ../ && rm -rf DemoStuff


rm -rf DemoStuff/bin DemoStuff/obj DemoStuff.Tests/bin DemoStuff.Tests/obj
dotnet clean
dotnet restore

# Otherwise I get weird errors about:
#/usr/share/dotnet/sdk/2.0.2/Sdks/Microsoft.NET.Sdk/build/Microsoft.PackageDependencyResolution.targets(323,5): error : Assets file '/vagrant/XenonCron/DemoStuff.Tests/C:/Users/chris/Desktop/NETCORE/XenonCron/DemoStuff.Tests/obj/project.assets.json' not found. Run a NuGet package restore to generate this file. [/vagrant/XenonCron/DemoStuff.Tests/DemoStuff.Tests.csproj]
#/usr/share/dotnet/sdk/2.0.2/Sdks/Microsoft.NET.Sdk/build/Microsoft.PackageDependencyResolution.targets(165,5): error : Assets file '/vagrant/XenonCron/DemoStuff.Tests/C:/Users/chris/Desktop/NETCORE/XenonCron/DemoStuff.Tests/obj/project.assets.json' not found. Run a NuGet package restore to generate this file. [/vagrant/XenonCron/DemoStuff.Tests/DemoStuff.Tests.csproj]
cd DemoStuff && dotnet build && dotnet restore && cd ..
cd DemoStuff.Tests && dotnet build && dotnet restore && cd ..

#dotnet build
dotnet publish -c Release
dotnet DemoStuff/bin/Release/netcoreapp2.0/publish/DemoStuff.dll


# https://unix.stackexchange.com/questions/2677/recovering-accidentally-deleted-files
# http://blog.nullspace.io/recovering-deleted-files-using-only-grep.html
# https://unix.stackexchange.com/questions/90036/grep-memory-exhausted
sudo grep -a -C 10 -F --null-data 'Console.WriteLine("inserted via dapper: {0}",' /dev/sda1 > search.txt


sudo apt-get install -y redis-tools postgresql-client
redis-cli  # set key3 value3 ; get key3
psql -h localhost -U postgres -d postgres

DROP TABLE IF EXISTS data, things;

CREATE TABLE data (
 id serial PRIMARY KEY,
-- email VARCHAR (255) UNIQUE NOT NULL,
 name VARCHAR (255) NOT NULL
);

CREATE TABLE things (
 id UUID PRIMARY KEY,
 name VARCHAR (255) NOT NULL,
 enabled BOOLEAN NOT NULL DEFAULT 'f',
 created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
 updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

echo "POSTGRES_CONNECTION=Host=localhost;Username=postgres;Password=postgres;Database=postgres" > DemoStuff/.env



sudo apt-get install -y kafkacat

kafkacat -P -b localhost -t test

kafkacat -C -b localhost -t test -f 'Topic %t[%p], offset: %o, key: %k, payload: %S bytes: %s\n'

