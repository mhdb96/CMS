# YGL-Projcet-1

Course Management System

YENİ BİR SİSTEM İÇİN KULLANILACAKSA

-Server'a MSSQL Server'ı kur

-CMS.bak dosyasını MSSQL Server'a restore et

-MSSQL Server hesabı oluştur ve CMS veri tabanına bağlanabilsin

-TCP-IP protokolünü SQL Server Configuration Manager > SQL Server Network Configuration > 
Protocols for (Uygulamak İstenen SQL Server Nesnesi)'dan Etkinleştir.

-TCP/IP > Özellikler > IP Adress > IPAll > TCP Port'a değer ver 

-Bilgisayarın IP adresini statik yap

-Güvenlik Duvarı > Gelen Kurallar > Yeni Kural > Bağlantı Noktası > TCP  /  Belirli Yerel Bağantı Noktası = TCP Port'a verilen değer > 
Bağlantıya İzin Ver > Etkin Alan  /  Özel  /  Ortak > Ad ve Açıklama Girin > Son

-Setup'a Bas > Database Settings > Verileri doldur > Bağlan



MEVCUT OLAN SİSTEME BAĞLANILACAKSA

-Sunucunun ağına bağlan

-Setup'a bas > Database settings bilgilerini sunucu yöneticisinden öğren > Verileri doldur > Bağlan 
