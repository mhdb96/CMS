# How To Deploy

## 🆕 YENİ BİR SİSTEM İÇİN KULLANILACAKSA

* Server'a MSSQL Server'ı kur
* CMS.bak dosyasını MSSQL Server'a restore et
* MSSQL Server hesabı oluştur ve CMS veri tabanına bağlanabilsin
* TCP-IP protokolünü SQL Server Configuration Manager &gt; SQL Server Network Configuration &gt; Protocols for \(Uygulamak İstenen SQL Server Nesnesi\)'dan Etkinleştir.
* TCP/IP &gt; Özellikler &gt; IP Adress &gt; IPAll &gt; TCP Port'a değer ver
* Bilgisayarın IP adresini statik yap
* Güvenlik Duvarı &gt; Gelen Kurallar &gt; Yeni Kural &gt; Bağlantı Noktası &gt; TCP / Belirli Yerel Bağantı Noktası = TCP Port'a verilen değer &gt; Bağlantıya İzin Ver &gt; Etkin Alan / Özel / Ortak &gt; Ad ve Açıklama Girin &gt; Son
* Setup'a Bas &gt; Database Settings &gt; Verileri doldur &gt; Bağlan

## 📂 MEVCUT OLAN SİSTEME BAĞLANILACAKSA

* Sunucunun ağına bağlan
* Setup'a bas &gt; Database settings bilgilerini sunucu yöneticisinden öğren &gt; Verileri doldur &gt; Bağlan

