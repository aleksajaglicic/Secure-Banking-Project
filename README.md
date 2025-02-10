# Secure banking project

OIBIS projekat je aplikacija koja omogućava bezbednu komunikaciju između klijenata i banke. Projekat je razvijen u **C#-u**, koristeći **WCF (Windows Communication Foundation)** tehnologiju, **Windows Autentifikaciju** i **sertifikate** za autentifikaciju i enkripciju. Fokusira se na obezbeđivanje sigurne i efikasne komunikacije između klijenata i banke, implementirajući napredne bezbednosne mehanizme.

## Funkcionalnosti

Projekat implementira sledeće ključne funkcionalnosti:

1. **Sigurna komunikacija** između klijenata i banke korišćenjem WCF (Windows Communication Foundation) tehnologije.
2. **Autentifikacija korisnika** pomoću **Windows Autentifikacije**, koja omogućava sigurno i jednostavno prijavljivanje.
3. **Upotreba sertifikata** za autentifikaciju i enkripciju komunikacije. Sertifikati se koriste za proveru identiteta klijenata i servera, kao i za generisanje i verifikaciju digitalnih potpisa.
4. **3DES kriptografija** za enkripciju podataka prilikom prenosa, čime se obezbeđuje sigurnost i zaštita podataka.
5. **Digitalni potpisi** za poruke, koji omogućavaju verifikaciju autentičnosti i integriteta prenetih podataka.
6. **X509Certificates** biblioteka za generisanje i proveru sertifikata.
7. **Audit funkcionalnosti** za logovanje i praćenje uspešnosti operacija, čime se omogućava nadzor nad svim transakcijama i komunikacijama između klijenata i banke.

## Tehnologije

- **C#**: Glavni programski jezik za razvoj aplikacije.
- **WCF** (Windows Communication Foundation): Tehnologija za razvoj sigurne i efikasne komunikacije između klijenata i servera.
- **3DES Kriptografija**: Korišćena za enkripciju podataka i obezbeđivanje sigurnosti prenosa.
- **X509Certificates**: Biblioteka za rad sa sertifikatima, korišćena za generisanje i verifikaciju sertifikata.
- **Windows Autentifikacija**: Korišćena za autentifikaciju korisnika.
- **Audit funkcionalnosti**: Implementirane za logovanje operacija i praćenje uspešnosti.

## Ključne funkcionalnosti

1. **Sigurna autentifikacija**: Korišćenje sertifikata za autentifikaciju korisnika i servera, čime se obezbeđuje verifikacija identiteta i sprečava neovlašćen pristup.
2. **Enkripcija podataka**: Korišćenje 3DES kriptografije za siguran prenos podataka između klijenata i banke.
3. **Digitalni potpisi**: Kreiranje i verifikacija digitalnih potpisa koji omogućavaju sigurnost i integritet poruka.
4. **Audit logovi**: Praćenje svih operacija u sistemu kako bi se obezbedila transparentnost i sigurnost transakcija.
5. **Upotreba sertifikata**: Generisanje i verifikacija X.509 sertifikata za autentifikaciju i enkripciju.

## Instalacija

Da biste pokrenuli projekat na svom računaru, pratite sledeće korake:

### 1. Klonirajte repo

Klonirajte repo sa GitHub-a:

```bash
git clone https://github.com/aleksajaglicic/oibis.git
cd oibis
