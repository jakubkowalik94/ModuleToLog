# ModuleToLog

Pliki kodu znajdują się w folderze ModuleToLog.

1. Klasa LogToFile jest właściwą klasą wykonującą zadanie projektowe. Klasa Program służyła do uruchamiania, testowania itp. Konstruktor
klasy LogToFile ustawia wartości zmiennych odpowiadających m.in. za rozmiar bufora oraz ścieżkę do pliku.

2. Funkcje zapisywania loga do pliku można uzyskać tworząc obiekt klasy LogToFile i wywołując funkcję TextToFile(string text).
Wybrano taką opcję, ponieważ podawanie ścieżki do pliku w konstruktorze jest bardziej uniwersalne i pozwala na stworzenie kilku logów do 
różnych plików i ścieżek. Gdyby ścieżkę ustawić na stałe, można byłoby wykorzystywać funkcję statyczną i wywoływać funkcję bez tworzenia obiektó tej klasy.

3. void TextToFile(string text) - Funkcja publiczna, przez którą zewnętrzna klasa może wpisać informację do logu. Ustawiony jest tam 
semafor _fileSemaphore, który chroni przed konfliktem dostępu do tego samego pliku i listy bufora przez kilka różnych wątków. Dodaje stringi do bufora, który jest typu List<string>. Jego rozmiar jest zadeklarowany przez parametr w konstruktorze. Po jego przepełnieniu następuje zapis danychdo pliku. Dzięki temu operacja na plikach będzie występowała mniej razy.

void ToFile() - Funkcja, która realizuje sam proces zapisu bufora do pliku.

string AddDate(string text)- Funckja przygotowuje odpowiedni format stringu do zapisu, po przez dodanie daty i czasu, kiedy log został 
wykonany.

4.W klasie TextToFile na początku w bloku try pojawia się funkcja _fileSemaphore.WaitOne(), która ma za zadanie zabezpieczyć sekcję 
krytyczną kodu, która znajduje się za tą funkcją przed dostępem wielu wątków jednocześnie. Następnie wykonywane jest wpisywanie podanego
tekstu do bufora. Następnie jest sprawdzany warunek przepełnienia bufora oraz czy zmienna _forceBufferDump "wymusza" zapisanie zawartości 
bufora do pliku. Samo zapisanie odbywa się w funkcji ToFile po przez funkcję System.IO.File.AppendAllLines . Następnie następuje sekcja 
catch,która ma za zadanie obsługę wyjątków, którą można znacznie rozbudować, ponieważ w obecnej formie ma za zadanie jedynie wyświetlenie 
wyjątku w konsoli. Na końcu w sekcji finally następuje zwolnienie semofora, dzięki funkcji _fileSemaphore.Release().

5.
-Wielowątkowość może powodować konflikty przy dostępie do plików, co może spowodować utratę danych.
-Różne wątki nie mogą modyfikować tych samych zmiennych klasy, bo też mogą powodować błędy.
-Przez to, że dane są buforowane, trzeba pamiętać, że plik nie będzie od razu aktualizowany, co trzeba uwzględnić deklarując rozmiar 
bufora. Jeżeli chcemy natychmiast zapisywać do pliku, można ustawić rozmiar bufora na 0. Poza tym trzeba pamiętać o zapisaniu zawartości bufora w przypadku, gdy zakończy się czas życia obiektu,a bufor będzie niepełny, aby nie stracić danych już w nim zapisanych.
