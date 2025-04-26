boiled it down to JS

var getItalianPropertyList = function() {

    function copyToClipboard(text) {
        if (window.clipboardData && window.clipboardData.setData) {
            return clipboardData.setData("Text", text);
        } else if (document.queryCommandSupported && document.queryCommandSupported("copy")) {
            var textarea = document.createElement("textarea");
            textarea.textContent = text;
            textarea.style.position = "fixed";  
            document.body.appendChild(textarea);
            textarea.select();
            try {
                return document.execCommand("copy");
            } catch (ex) {
                console.warn("Copy to clipboard failed.", ex);
                return false;
            } finally {
                document.body.removeChild(textarea);
            }
        }
    }

    var list = Array.from(document.querySelectorAll('.nd-list__item.in-searchLayoutListItem'));
    var line = '';

    list.forEach(function(item) {
        var link = item.querySelector('a.in-listingCardTitle').getAttribute('href');
        var address = item.querySelector('a.in-listingCardTitle').innerText;

        // Remove "Appartamento all'asta" from the address if present
        address = address.replace(/Appartamento all'asta /, '').trim();

        // Price extraction - Flatten the structure and extract directly from the first span
        var priceElement = item.querySelector('div.in-listingCardPrice span');
        var price = 'N/A';

        if (priceElement) {
            price = priceElement.innerText.trim();

            // Remove "da " and the "€" symbol, then clean up the number
            price = price.replace(/^da\s*/, '').replace('€', '').trim();

            // Remove thousands separator (.) and replace the decimal comma (,) with a dot (.)
            price = price.replace(/\./g, '').replace(/,/g, '.'); // Remove periods and replace commas with dots

            // Ensure the price is a valid number and convert it to integer
            price = Math.round(parseFloat(price)).toString();  // Convert to integer
        } else {
            console.warn("Price element not found for listing: " + link);
        }

        var features = item.querySelectorAll('div.in-listingCardFeatureList__item');
        var rooms = 'N/A', area = 'N/A';

        features.forEach(function(feature) {
            if (feature.innerText.includes('locale') || feature.innerText.includes('locali')) {  // "locale" or "locali" in Italian
                rooms = feature.innerText.replace(/[^\d]/g, '').trim(); // Extract just the number
            }
            if (feature.innerText.includes('m²')) {  // Extracting the area in square meters
                area = feature.innerText.replace(/[^\d]/g, '').trim(); // Extract just the number
            }
        });
        
        var town = address.split(',').pop().trim(); // Assuming the town is the last part of the address
        var postcode = address.split(' ').slice(-2).join(' '); // Assuming the postcode is the second-to-last part of the address
        
        var date = new Date().toISOString();  // Current date for the listing
        
        line += `NULL\t${link}\t${address}\t${postcode}\t${town}\t${price}\t${area}\t${rooms}\t${date}\n`;
    });

    copyToClipboard(line);
};

getItalianPropertyList();
