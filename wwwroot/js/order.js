$(document).ready(function(){
    createoption();

    function createoption(data){
        var quantity=$("#plist").val();
        console.log(quantity)
        var url="/product/quantity/"+quantity
        $("#quantity").empty();
        $.get(url, function(data){
            if(data.quantity>0){
                for(var i=1;i<=data.quantity;i++){
                    var option=$("<option/>")
                    .val(i)
                    .text(i)
                    .appendTo("#quantity")
                }
            } else{
                    var option=$("<option/>")
                    .val(0)
                    .text(0)
                    .appendTo("#quantity")                
            }

        },'json')

    }

    $("#plist").change(function(){
        createoption();
    })


})	

