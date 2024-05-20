document.addEventListener('DOMContentLoaded', (event) => {
    // Ensure the form element is available
    const form = document.querySelector('form');
    if (form) {
      form.addEventListener('submit', (event) => {
        event.preventDefault();
        
        // Retrieve input values
        const bishValue = document.getElementById('bish').value;
        const boshValue = document.getElementById('bosh').value;
        const countValue = document.getElementById('count').value;
  
        // Convert values to numbers
        const bish = parseInt(bishValue);
        const bosh = parseInt(boshValue);
        const count = parseInt(countValue);
  
        generateBishBoshSequence(bish, bosh, count);
      });
    } else {
      console.error('Form element not found!');
    }
  });
  
  function generateBishBoshSequence(bish, bosh, count) {
    const resultBox = document.getElementById('result-box');
    resultBox.innerHTML = "<h3>Resulting Bish Bosh Sequence</h3><ul>";
    for(var i = 1; i <= count; i++) {
        var output;
        if (i % bish == 0) {
            if (i % bosh == 0) {
                output = "Bish-Bosh";
            }
            else
            {
                output = "Bish";
            }
        }
        else if (i % bosh == 0) {
            output = "Bosh";
        }
        else {
            output = i;
        }
        resultBox.innerHTML += "<li>" + output + "</li>";
    }
    resultBox.innerHTML += "</ul>";
  }