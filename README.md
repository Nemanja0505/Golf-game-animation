# Golf-game-animation

Projektni zadatak 15.2–Golf

Modelovanje statičke 3D scene (prva faza): 

•	Uključiti testiranje dubine i sakrivanje nevidljivih površina. Definisati projekciju u perspektivi (fov=45, near=0.5, a vrednost far po potrebi) i viewport-om preko celog prozora unutar Resize()metode.
•	Koristeći AssimpNet bibloteku i klasu AssimpScene, učitati model golf palice.Ukoliko je model podeljen u nekoliko fajlova, potrebno ih je sve učitati i iscrtati. Skalirati model, ukoliko je neophodno, tako da bude vidljiv u celosti.
•	Modelovati sledeće objekte: 
•	podlogu koristeći GL_QUADS primitivu,
•	rupu koristeći Disk klasu i,
•	šipku(koristeći Cylinder klasu ) sa zastavicom(GL_TRIANGLES) - flagpole
•	lopticu za golf koristeći Sphere klasu
•	Ispisati 3D tekst crvenom bojom u gornjem levom uglu prozora. Font je Tahoma, 10pt. Tekst treba da bude oblika: 
Predmet: Racunarska grafika 
Sk.god: 2020/21
Ime: <ime_studenta>
Prezime: <prezime_studenta>
Sifra zad: <sifra_zadatka>


Predmetniprojekat - faza 1 sačuvati pod nazivom: PF1S15.2. Obrisati poddirektorijume bin i obj. Zadaci se brane na vežbama, pred asistentima.
Vreme za izradu predmetnog projekta – faze 1 su dve nedelje.
Predmetniprojekat – faza 1 vredi 15 bodova.Način bodovanja je prikazan u tabeli.

| Šifra kriterijuma	| Bodovi	| Opis |
|-------------------|---------|------|
|CVP	| 3	| Kreiran prozor. Uključeno testiranje dubine i sakrivanje nevidljivih površina. Projekcija, kliping volumen i viewport podešeni. |
|M	| 9 |	Adekvatno učitani ili modelovani pa zatim prikazani mesh modeli. |
|T |	3	| Ispisan tekst adekvatnim fontom, bojom, i na adekvatnoj poziciji.|



Definisanje materijala, osvetljenja, tekstura, interakcije i kamere u 3D sceni  (druga faza):

•	Uključiti color tracking mehanizam i podesiti da se pozivom metode glColor()definiše ambijentalna i difuzna komponenta materijala.
•	Definisati tačkasti svetlosni izvorbele boje i pozicionirati ga iznad podloge. Svetlosni izvor treba da bude stacionaran (tj. transformacije nad modelom ne utiču na njega). Definisati normale za podlogu. Za Quadric objekte podesiti automatsko generisanje normala.
•	Za teksture podesiti wrapping da bude GL_REPEAT po obema osama. Podesiti filtere za teksture tako da se koristi najbliži sused filtriranje. Način stapanja teksture sa materijalom postaviti da bude GL_REPLACE. 
•	Podlozi pridružiti teksturu trave(slika koja se koristi je jedan segment trave). Šipci pridružiti teksturu žute plastike. Sferi pridružiti teksturu koja odgovara loptici za golf. Pritom obavezno skalirati teksturu (shodno potrebi). Skalirati teksturu korišćenjem Texture matrice.
•	Pozicionirati kameru tako da je rupa u centru scene. Koristiti gluLookAt() metodu.
•	Pomoću ugrađenih WPF kontrola, omogućiti sledeće:
•	pomeranje pozicije rupe u okviru podloge,
•	izbor faktora uniformnog skaliranja loptice, i
•	izbor boje difuzne komponente tačkastog svetlosnog izvora.
•	Omogućiti interakciju sa korisnikom preko tastature: sa F2 se izlazi iz aplikacije, tasterima  
E/Dvrši se rotacija za 5 stepeni oko horizontalne ose, tasterima S/Fvrši se rotacija za 5 stepenioko vertikalne ose, a tasterima +/-približavanje i udaljavanje centru scene. Ograničiti rotaciju tako da se nikada ne vidi donja strana podloge. Dodatno ograničiti rotaciju oko horizontalne ose tako da scena nikada ne bude prikazana naopako.

Kreirati animaciju udaranja loptice palicom i pogotka u rupu..U toku animacije, onemogućiti interakciju sa korisnikom (pomoću kontrola korisničkog interfejsa i tastera). Animacija se može izvršiti proizvoljan broj puta i pokreće se pritiskom na tasterV. 

Neophodne teksture pronaći na internetu. Predmetni projekat - faza 2 sačuvati pod nazivom: PF2S15.2. Obrisati poddirektorijume bin i obj. Zadaci se brane na vežbama, pred asistentima.
Vreme za izradu predmetnog projekta – faze 2 su četiri nedelje. Predmetni projekat – faza 2 vredi 35 bodova.Način bodovanja je prikazan u tabeli.

| Šifra kriterijuma |	Bodovi	| Opis |
|-------------------|---------|------|
| M |	2	| Podešeni materijali u skladu sa zahtevima zadatka. |
| S	| 8	| Definisani svetlosni izvori, u skladu sa zahtevima zadatka. |
| T	| 8	| Učitane, dodeljene, podešene, i mapirane teksture, u skladu sa zahtevima zadatka. |
| K	| 2	| Definisana kamera. |
| I	| 7	| Omogućena interakcija, u skladu sa zadatkom. |
| A	| 8	| Realizovana animacija, u skladu sa zadatkom. |
