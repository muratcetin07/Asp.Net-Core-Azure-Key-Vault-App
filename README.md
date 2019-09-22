#DotNetCoreAzureKeyVaultApp

EN : How to Use Azure KeyVault on .Net Core 
How to access your azure key vault parameters on dotnet core web application. In this example shows us to get your database
connection string from microsoft azure key vault to dotnet core web app. There are two approaches, first how to access 
your azure key vault  parameters on development and staging environments with your azure client-id, client-secret and key-vault secrets.
Second : how to access application levels to azure key vault. For example your production environment access to azure key vault 
with key-vault identifier.

------------------------------------------------------------

TR : Bu uygulamada .Net Core ile Azure Key Vault kullanımı bir veritabanı connection stringinin azure key vaulta tanımlanıp,
azure üzerinden uygulama içine enjekte edilmesi fikri örneklendirilimiştir ve bu bakış açısı ile Development / Staging(Test) 
ve Production branchlerinde azure keyvault kullanımı ele alınmıştır.

Azure key vault iki farklı yaklaşım ile değelendirilmiştir ; 
1-) dev ve test ortamları için ; azure portaldee oluşturduğunuz uygulamanıza ait 
client-id / client secret  / key-vault bilgileriniz ile api isteği şeklinde azure dan gerekli vault değerlerinizi almak 

2-) production ortamı için ; azure portalde key vault tanımınıza ilgili prod branchinizin (azure üzerinde prod deployment slotu) 
erişim yetkisi olması ve sadece uygulama bu brench üzerinde ve çalışa anında keyvault değerine ulaşamabilmesi. 
-----------------------------------------------------------------------------------


