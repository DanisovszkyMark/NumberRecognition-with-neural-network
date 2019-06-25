# NumberRecognition with neural network
Az alábbi projekt a neurális hálózatok felhasználásának egy lehetőségét mutatja be, amik segítségével kézzel írt számjegyeket is lehetőségünk van felismerni.

Fontos, hogy a projekt célja nem egy rugalmasan felhasználható és módosítható program létrehozása, hanem egy "bedrótozott" hálózat felhasználása számjegyfelismerésre. A hálózatot lehessen tanítani, felhasználni, menteni illetve beolvasni külső fileból. Ezen kívül további funkciók implementálása nem cél.

A projekt megvalósításának éve: 2018

**Tervezet**

A felületnek lehetőséget kell biztosítania az alábbiaknak:

-Kép importálásának lehetősége. (fix, 27*27 pixelből álló képek)

-Számjegy rajzolása valós időben.

-Számjegy felismerése és ez alapján egy rangsor előállítása, melyik számjegyre hasonlít még a rajzolt / tallózott kép.

Ezen kritériumok alapján az alábbi felépítés megvalósítása elegendő:

<p align="center">
<img src="Designs/MainDesign.png" width="600">
</p>

**Az elkészült szoftver bemutatása**

A program megnyitásakor az alap kezdő felület jelenik meg.

Ez a felület tartalmazza a következőket:

-Rajztábla

-Rajzolt szám mentésének lehetősége a későbbi tanulási példák felhasználásának céljából

-A rajzolt szám felismerésének megpróbálása

-A rajztábla törlése

-A kiértékelés eredményének megjelenítésére szolgáló elem

-Jelző elem, mely jelzi, hogy az aktuális művelet hol tart a végrehajtásban (pl tanulás)

<p align="center">
<img src="Designs/Képernyőképek/Alap.png" width="600">
</p>

Egy alap, még nem tanított háló felhasználásával a kép felismerése:

<p align="center">
<img src="Designs/Képernyőképek/TanulasNelkul.png" width="600">
</p>

Van lehetőségünk tanítani az alapértelmezetten beágyazott neurális hálózatunkat, növelve annak felismerési teljesítményét:

<p align="center">
<img src="Designs/Képernyőképek/Tanítás.png" width="600">
</p>

A tanítási folyamatot egy ProgressBar segítségével követketjük nyomon:

<p align="center">
<img src="Designs/Képernyőképek/TanításKözben.png" width="600">
</p>

A tanítás után nőtt a teljesítmény:

<p align="center">
<img src="Designs/Képernyőképek/TanításUtán.png" width="600">
</p>

Lehetőségünk van még menteni a már betanított neurális hálózatot későbbi felhasználás céljából:

<p align="center">
<img src="Designs/Képernyőképek/SaveNetwork.png" width="600">
</p>

Vagy beolvasni neurális hálózatot korábban lementett fileból. 

<p align="center">
<img src="Designs/Képernyőképek/LoadNetwork.png" width="600">
</p>

Néhány további kép a felismerés sikerességéről:

<p align="center">
<img src="Designs/Képernyőképek/Eredmeny1.png" width="600">
</p>

<p align="center">
<img src="Designs/Képernyőképek/Eredmeny2.png" width="600">
</p>

**Továbbfejlesztési lehetőségek**

-Neurális hálózat felépítésének módosítása.

-Cserélhető forrás illetve cél fájlok
