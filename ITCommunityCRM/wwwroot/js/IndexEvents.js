
var Create = new bootstrap.Modal(document.getElementById('Create'));

var Edit = new bootstrap.Modal(document.getElementById('Edit'));

var btnsEdit = document.querySelectorAll('#btnEdit')


btnsEdit.forEach((el)=>{
    
    el.addEventListener('click',(el) =>{        
        var btnEdit = el.target;
        var parent = btnEdit.parentElement.parentElement
        
        var nickName = parent.getElementsByClassName('infoTh')[0].innerText
        var type = parent.getElementsByClassName('infoTh')[1].getAttribute('data-name');       
        var startTime = parent.getElementsByClassName('infoTh')[2].getAttribute('data-name');
        var endTime = parent.getElementsByClassName('infoTh')[3].getAttribute('data-name');
        var description = parent.getElementsByClassName('infoTh')[4].getAttribute('data-name');
        var id = parent.getElementsByClassName('infoTh')[5].getAttribute('data-name');
        debugger

        document.getElementById('editName').value = nickName;
        document.getElementById('editId').value = id;
        document.getElementById('editSelect').value = type;
        document.getElementById('editDescription').value = description;
        document.getElementById('editStart').value = startTime;
        document.getElementById('editEnd').value = endTime;
        
        
    })
})




$(document).ready(function () {

    (function ($) {

        $('#filter').keyup(function () {

            var rex = new RegExp($(this).val(), 'i');
            debugger
            $('.searchable tr').hide();
            $('.searchable tr').filter(function () {
                return rex.test($(this).text());
            }).show();

        })

    }(jQuery));

});
async function main(){
    let currentPage = 0;
    let selector = document.getElementById('currentMaxPage');
    let btnNext = document.getElementById('next');
    let btnBack = document.getElementById('back');
    let pageNum = document.getElementById('pageNum');




    let rows = Number(selector.value);
    
    
    
    const postsData = Array.from(document.getElementsByTagName('tr')).slice(1);
    var tBody = document.getElementById('tBody');
    tBody.innerHTML = "";

    btnNext.addEventListener('click',(el)=>{
        debugger        
        if((postsData.length/rows)>=currentPage+1){
            debugger
            currentPage++;
            pageNum.innerText = currentPage + 1;
            displayList(tBody,postsData,rows,currentPage)
        }
    })

    btnBack.addEventListener('click',(el)=>{
        debugger
        if(currentPage >= 1){
            currentPage--;
            pageNum.innerText = currentPage + 1;
            displayList(tBody,postsData,rows,currentPage)
        }
    })

    selector.addEventListener('change',(el) => {
       
        var selector = el.target
        rows = Number(selector.value);        
        displayList(tBody,postsData,rows,currentPage)  
    })
    displayList(tBody,postsData,rows,currentPage)   
    
    function displayList(tBody,arrData, rowPerPage, page) {
        const start = rowPerPage * page;
        const end = start + rowPerPage;
        const paginatedData = arrData.slice(start, end);  
        tBody.innerHTML = "";              
        paginatedData.forEach((el) => {
            
            tBody.append(el);            
          })
    }
}
main();


