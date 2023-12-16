
(function () {
    
    var formsEdit = document.querySelectorAll('.needs-validation-edit')


    Array.prototype.slice.call(formsEdit)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })
 
    var forms = document.querySelectorAll('.needs-validation-create')

 
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                debugger
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })
})()
  
   