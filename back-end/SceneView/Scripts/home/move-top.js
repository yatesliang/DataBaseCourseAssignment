/* UItoTop jQuery Plugin 1.2 | Matt Varone | http://www.mattvarone.com/web-design/uitotop-jquery-plugin */
(function($){$.fn.UItoTop=function(options){var defaults={text:'To Top',min:200,inDelay:600,outDelay:400,containerID:'toTop',containerHoverID:'toTopHover',scrollSpeed:1200,easingType:'linear'},settings=$.extend(defaults,options),containerIDhash='#'+settings.containerID,containerHoverIDHash='#'+settings.containerHoverID;$('body').append('<a href="#" id="'+settings.containerID+'">'+settings.text+'</a>');$(containerIDhash).hide().on('click.UItoTop',function(){$('html, body').animate({scrollTop:0},settings.scrollSpeed,settings.easingType);$('#'+settings.containerHoverID,this).stop().animate({'opacity':0},settings.inDelay,settings.easingType);return false;}).prepend('<span id="'+settings.containerHoverID+'"></span>').hover(function(){$(containerHoverIDHash,this).stop().animate({'opacity':1},600,'linear');},function(){$(containerHoverIDHash,this).stop().animate({'opacity':0},700,'linear');});$(window).scroll(function(){var sd=$(window).scrollTop();if(typeof document.body.style.maxHeight==="undefined"){$(containerIDhash).css({'position':'absolute','top':sd+$(window).height()-50});}
if(sd>settings.min)
$(containerIDhash).fadeIn(settings.inDelay);else
        $(containerIDhash).fadeOut(settings.Outdelay);
});
};
})(jQuery);
(window), $(function () {
    function getNotes() {
        $(".loadMoreNote").hide(), $(".comment .loading").show(), $.ajax({
            url: "/home/getMoreNotes",
            type: "GET",
            dataType: "json",
            data: {
                offset: notesOffset
            }
        }).done(function (data) {
            $(".loadMoreNote").show(), $(".comment .loading").hide(), notesOffset += 20;
            var elems = [];
            data.items.forEach(function (note) {
                var prefix = note.pc[0],
                    imgArr = eval(prefix.images),
                    imageIndex = prefix.image_index;
                if (imgArr.length == 0)
                    return;
                var item = $('<a class="yinxiang" href="' + prefix.url + '" title="" target="_blanket">' + '<div class="img-cnt">' + '<img src="//img.chufaba.me/' + imgArr[imageIndex] + '" alt="">' + '<p class="photo-num">' + imgArr.length + "P</p>" + "</div>" + '<div class="txt-cnt">' + '<p class="txt">' + prefix.desc + "</p>" + "</div>" + '<div class="note-bottom">' + '<span class="fl"><i class="icon-location"></i>' + prefix.poi_name + "</span>" + '<span class="fr">' + prefix.username + "</span>" + "</div>" + "</a>");
                notesOffset !== 20 ? (elems.push(item), $("#JS-yinxiang-cont").append(item).imagesLoaded(function () {
                    $("#JS-yinxiang-cont").masonry("appended", item)
                })) : $("#JS-yinxiang-cont").append(item)
            });
            if (notesOffset == 20) {
                var $container = $("#JS-yinxiang-cont");
                $container.imagesLoaded(function () {
                    $container.masonry({
                        itemSelector: ".yinxiang",
                        isAnimated: !0
                    })
                })
            }
        }).fail(function () {
            console.log("error")
            alert("fail")
        }).always(function () {
            console.log("complete")
            alert("complete")
        })
    }
})
var notesOffset = 0;

