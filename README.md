# Beskriving

Detta program är ett projektarbete till kursen: Programmeringsteknik C#.

FirstApp: Användare
SecondApp: Administratör

Vi ska skapa ett butiksprogram där användaren har möjlighet att beställa produkter från en uppsättning av produkter genom ett GUI.
Administratörer ska kunna göra ändringar i produktutbudet genom ett annat GUI. Administratörer kan modifiera produktutbudet och 
koppla på bild till en produkt. I detta program kan administratören endast välja bilder från en uppsättning av bilder.

En produkt ska innehålla Namn, Pris, Beskrivning och bild.
Samtliga produkter läses in från en CSV-fil. Bilderna läses in från en separat CSV-fil. 

Programmet ska innehålla rabattkoder som användaren ska kunna använda (max 1 rabattkod per beställning). Likaså ska administratörer 
kunna modifiera rabattkoder. När en modifiering har gjorts och sparats så sparas filen i form av CSV-fil till Windows\Temp mappen. 

En rabatt innehåller: Rabattkod och en Procentsats som dras av totalkostnaden i användarens varukorg när en beställning görs.

Användaren har möjlighet att spara sin varukorg med valda produkter, till en CSV-fil som sparas i Windows\Temp mappen, och kan därefter stänga ner programmet. 
Nästa gång programmet startas så visas den sparade varukorgen kvar för användaren.

Programmet kollar om det finns en sparad varukorg från användaren i Temp mappen. Om så är fallet så läser programmet in den sparade varukorgen. 
Programmet kollar även om administratör gjort någon ändring i produktutbudet / rabattkoder och läser då in dessa ändringar som sedan visas för användaren.

Se Screenshots i repository för programmet: https://github.com/Orhan92/EndProject/tree/master/Screenshots
