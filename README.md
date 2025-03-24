# Bot Robocode Tank Royale dengan Algoritma Greedy

Repositori ini berisi 4 bot Robocode Tank Royale yang diimplementasikan menggunakan algoritma greedy. Keempat bot ini dibuat untuk memenuhi Tugas Besar 1 IF2211 Strategi Algoritma di Institut Teknologi Bandung.

## Deskripsi Bot
### Bot Adudu
Bot Adudu adalah bot yang mementingkan tembakan ke bot musuh dan bergerak di jalur yang berbentuk kotak di bagian tengah arena apabila tidak ada gangguan eksternal. Bot ini terus menerus memindai bot musuh dan akan mundur jika terkena peluru atau bot lain.
### Bot Straight
Bot Straight adalah bot yang bergerak secara horizontal lurus di tengah arena secara bolak-balik sambil memutarkan meriam dan pemindai secara 360 derajat terus menerus dan langsung menembak ketika menemukan bot lain dalam jangkauan pemindai.
### Bot Smart_Calendar1874
Bot Smart_Calendar1874 adalah bot yang diam mendekati dinding namun tetap terpisah sejauh suatu margin lalu terus memutarkan meriam dan pemindai 360 derajat dan menembak jika terdapat bot yang terpindai. Namun ketika tertembak, bot akan bergerak menyusuri dinding arena sejauh 100 unit.
### Bot CircularMG
Bot CircularMG adalah bot yang bergerak dalam pola lingkaran untuk menghindari serangan peluru dari musuh dan mendeteksi musuh dengan lebih efektif serta menyesuaikan kekuatan tembakan berdasarkan energi musuh.

<div id="contributor">
  <strong>
    <h3>Dibuat oleh Kelompok 11 - Dunggulge Dunggulge</h3>
    <table align="center">
      <tr>
        <td>NIM</td>
        <td>Nama</td>
      </tr>
      <tr>
        <td>13522016</td>
        <td>Clarissa Nethania Tambunan</td>
      </tr>
      <tr>
        <td>13522050</td>
        <td>Mayla Yaffa Ludmilla</td>
      </tr>
      <tr>
        <td>13522058</td>
        <td>Noumisyifa Nabila Nareswari</td>
      </tr>
    </table>
  </strong>
</div>

## Tech Stack
- Robocode API
- C++

## Cara Menjalankan Program
Clone repositori ini dengan perintah berikut.
```shell
git clone https://github.com/numshv/Tubes1_Dunggulge-Dunggulge.git
```
Navigasi ke repositori yang sudah di-clone, lalu jalankan GUI Robocode Tank Royale.

```shell
java -jar robocode-tankroyale-gui-0.30.0.jar
```
Untuk setiap bot, navigasi ke direktori masing-masing bot dan jalankan perintah berikut.
```shell
./[nama bot].cmd
```
(untuk Windows) 
```shell
./[nama bot].sh
```
(Untuk Linux/macOS)

Di Robocode Tank Royale GUI, klik tombol "Config" lalu tombol "Bot Root Directories" dan masukkan directory yang berisi folder-folder bot.
<br />
<br />
Untuk menjalankan sebuah battle, klik tombol "Battle" lalu tombol "Start Battle". 
Akan muncul panel konfigurasi permainan dan bot-bot di dalam directory yang telah disetup pada proses konfigurasi akan otomatis muncul pada kotak kiri-atas.
<br />
<br />
Boot bot yang ingin Anda mainkan dengan select  bot yang ingin dimainkan pada kotak kiri-atas dan klik tombol “Boot →”.
Bot yang berhasil di-boot akan muncul pada kotak kanan-atas dan kiri-bawah.
<br />
<br />
Tambahkan bot ke dalam permainan dengan select bot yang ingin ditambahkan ke dalam permaianan pada kotak kiri-bawah dan klik tombol “Add →”.
Bot yang telah ditambahkan akan otomatis muncul pada kotak kanan-bawah.
<br />
<br />
Anda dapat memulai permainan dengan  menekan tombol “Start Battle”.

### Kontribusi
Apabila Anda ingin berkontribusi dalam projek ini, silakan fork repository ini dan gunakan feature branch. Pull requests akan diterima dengan hangat.
