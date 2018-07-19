// made by may in 2018.7.19 the front
//click a button to update the page
function showScore(score){
    var scoreImg = document.getElementById("scoreShow");
}
function updateDistrict(district){
    var xmlhttp;
    if(window.XMLHttpRequest){
        xmlhttp = new XMLHttpRequest();
    }else{
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function(){
        if(xmlhttp.readyState==4 && xmlhttp.status==200){
            document.getElementById("sceneShow").innerHTML = xmlhttp.responseText;
        }
    }
    xmlhttp.open("POST","/ajax/",true); //to do
    xmlhttp.send();
}

// made by may in 2018.7.19 the end 