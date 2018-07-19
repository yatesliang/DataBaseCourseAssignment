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
            //to do 
            //生成规则范例,单个cell，sceneshow有多个cell
            // <a class="cell" href="index.html">
			// 				<div class="img-show">
			// 					<img src="images/32.jpg">
			// 				</div>
			// 				<div class="txt">
			// 					<p class="name">name</p>
			// 					<div class="score">
			// 						<i class="socre-show" id="scoreShow"></i>
			// 						<span class="score-num">score-num</span>
			// 					</div>
			// 					<p class="address">address</p>
			// 				</div>
			// 			</a>
        }
    }
    xmlhttp.open("POST","/ajax/",true); //to do
    xmlhttp.send();
}

// made by may in 2018.7.19 the end 