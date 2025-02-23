# ITD Asg2 UntitledSupermarketSimulator ReadMe

## Game Mechanics

### Items Customers Can Purchase
- **Canned Tuna:** $3.55
- **Milk:** $5.50
- **Yogurt:** $3.00
- **Biscuits:** $3.50
- **Rice:** $6.00
- **Pasta:** $2.00
- **Flour:** $2.50
- **Sugar:** $3.00
- **Water:** $1.50
- **Bottled Drinks:** $2.50
- **Chocolate:** $5.50
- **Cookies:** $7.00
- **Juice:** $4.50
- **Wine:** $30.00
- **Cigarettes:** $10.00

### Cash Denominations in Register Slots
- **Dollar Notes:** $1, $2, $5, $10, $20
- **Coins:** $0.01, $0.05, $0.10, $0.20, $0.50

### How to Play

#### Barcode Scanner
- **Usage:** Grab to hold the barcode scanner and use the trigger to scan items.

#### Cash Register
- **Opening the Register:** When closed, grab and use the trigger to open the cash register (this will trigger the payment process).
- **Handling Money:** When opened (and if needed), grab the monetary denomination to remove it from the register to give the customer change.
- **Closing the Register:** To close, grab the drawer and use the trigger (this will complete the transaction).
- **Void Function:** In case of a double scan on an item, use the void button on the computer screen to remove the item.

#### Customers
- **ID Verification:** Check if the customer has a fake ID (using the website player).
- **Age Restriction:** If the customer is buying restricted items, check if they are underaged (using the website player).
- **Rejection:** If either a fake ID or underage is detected, reject the customer.
- **Payment:** If a valid customer does not provide enough money, request additional funds.
- **Change:** If a customer gives excess money, grab the correct amount of change and close the register to complete the transaction.

---

## Platforms / Hardware Required
- **Meta Quest VR Headset**
- A compatible computer or console that supports VR

---

## Limitations / Bugs
- **Floating Point Error:** Customer payments might trigger errors due to precision issues, leading to incorrect change calculations.
- **Transaction Tracking:** Checking for transactions involving invalid IDs or restricted sales to minors can disrupt the tracking of average transaction times.
- **Customer Persistence:** Customers leaving the store do not get destroyed.
- **Interaction Issues:** There may be instances where you suddenly cannot grab the cash register to start transactions.

---

## References
- **Scanner Beep:** [Pixabay - Store Scanner Beep](https://pixabay.com/sound-effects/store-scanner-beep-90395/)
- **NPC’s:** [City People Free Samples on Unity Asset Store](https://assetstore.unity.com/packages/3d/characters/city-people-free-samples-26044) *(see description: 6#description)*
- **Grocery Store:** [Grocery Store Environment on Unity Asset Store](https://assetstore.unity.com/packages/3d/environments/grocery-store-203239)
- **Barcode Scanner:** [Barcode Scanner 3D Model on Free3D](https://free3d.com/3d-model/barcode-scanner-88269.html)
- **Money:** [Free Money Currency Basic Pack on Sketchfab](https://sketchfab.com/3d-models/free--money-currency-basic-pack-4368d88ee80f453) *(ID: dbbdbf78784bc32ad)*
- **Wine Bottle:** [Wine Bottle 3D Model on Sketchfab](https://sketchfab.com/3d-models/wine-bottle-2303628f0243427bbe81c51dda60aab4)
- **Cigarette Pack:** [Marlboro Cigarettes Pack on Sketchfab](https://sketchfab.com/3d-models/marlboro-cigarettes-b8e612ccdb634e6388eeaf87ffbd46b3)
- **Door Model:** [Double Doors with Windows 3D Model on Sketchfab](https://sketchfab.com/3d-models/double-doors-with-windows-79ebb7d7e2ec415b9320f4680440ed0e)
- **Money Font:** [Dollar Bill Font on FontSpace](https://www.fontspace.com/dollar-bill-font-f18945)
- **Supermarket Title:** [ToySans Font on FontSpace](https://www.fontspace.com/toysans-font-f22064)
