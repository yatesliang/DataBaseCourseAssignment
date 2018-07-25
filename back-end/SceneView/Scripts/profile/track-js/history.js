var History = function($container, options) {
    if(typeof $container === 'undefined') return;

    var H = this;

    var defaults = {
        timelineLength : .65,    // length of timeline based on circle in decimal pct
        chapterSelector : '.ui-chapter',
        duration: 1,
        autoSwitchInterval: 3,
        currentIndex: 0,
        functions: {}
    };
    if(typeof options !== 'undefined') jQuery.extend(defaults, options);

    H.options = defaults;
    H.interval = new TimelineMax();
    H.$object = {
        container : $container,
        headline : $container.find('.chapter-headline'),
        copy : $container.find('.chapter-copy'),
        image : $container.find('.chapter-image'),
        timeline: {
            container : $container.find('.timeline-container'),
            svgContainer : $container.find('.timeline-container .ui-timelines'),
            svgTimeline : $container.find('.timeline-container .ui-timelines svg').last().find('circle'),
            chapters : $container.find('.timeline-container '+H.options.chapterSelector)
        }
    };
    //console.log('History objects', H.$object);

    init();

    function init() {
        if(_.isFunction(H.options.functions.onInit)) H.options.functions.onInit({timestamp:new Date()});
        init_circles();
        jQuery(document).on('click', H.options.chapterSelector, function(e) {
            chapterClick(e);
        });
        autoSwitch();
    }// init()

    function autoSwitch() {
        H.interval = new TimelineMax({loop:true});
        H.interval.to(window, H.options.autoSwitchInterval, {onComplete:function() {
            //H.options.currentIndex = get_next_index();
            //update_chapter();
            var nextIndex = get_next_index();
            //console.log('jump to next chapter', H.options.currentIndex, nextIndex);
            H.$object.timeline.chapters.eq(nextIndex).click();
            //H.interval.play();
        }});
    }// autoSwitch

    function chapterClick(e) {
        e.preventDefault();
        H.interval.kill();

        var $clicked = jQuery(e.currentTarget);
        ////console.log('CHAPTER CLICKED', $clicked);

        H.$object.timeline.chapters.removeClass('current-chapter');
        $clicked.addClass('current-chapter');
        //var timelinePCT = $clicked.attr('data-pct'),
        H.options.currentIndex = $clicked.index();
        //console.log('clicked index', chapterIndex);
        //update_timeline(timelinePCT);
        update_chapter();
        autoSwitch();
    }// chapterClick()
    
    function update_timeline(pct) {
        TweenMax.to(H.$object.timeline.svgTimeline, 1, {drawSVG:('0 '+pct+'%'), ease:Strong.easeOut});
    }

    function get_next_index() {
        var nextIndex = H.options.currentIndex +1;
        if(nextIndex >= H.$object.timeline.chapters.length) nextIndex = 0;
        return nextIndex;
    }

    function update_chapter(index) {

        var timelinePCT = H.$object.timeline.chapters.eq(H.options.currentIndex).attr('data-pct');
        update_timeline(timelinePCT);

        var tl = new TimelineMax();

        tl.staggerTo([H.$object.headline, H.$object.image, H.$object.copy], H.options.duration *.25, {autoAlpha: 0},.1);
        tl.add(function() {
            H.$object.headline.html(H.options.chapters[ H.options.currentIndex ].title);
            H.$object.copy.html(H.options.chapters[ H.options.currentIndex ].copy);
            H.$object.image.css({
                'background-image' : 'url(' + H.options.chapters[ H.options.currentIndex ].img.src + ')'
            });
        });
        tl.staggerTo([H.$object.headline, H.$object.image, H.$object.copy], H.options.duration *.25, {autoAlpha: 1, clearProps:'opacity, visibility'},.1);

    }// update_chapter()

    function init_circles() {
        // calculate maxRadius
        H.maxRadius = convert_degrees(H.options.timelineLength, 'pct-deg');

        // draw the base timeline circle to fit maxRadius
        H.$object.timeline.svgContainer.find('circle').each(function(i) {
            TweenMax.set(this, {drawSVG:(i > 0 ? '0% 0%' :  ('0% '+ (H.options.timelineLength *100) +'%'))});
        });

        // distribute chapters according to maxRadius
        circle_distribution(H.$object.timeline.chapters, H.maxRadius, {onElem:position_label});

        // rotate timeline Circles so they visually fit the copy on the left
        TweenMax.set(H.$object.timeline.svgContainer, {rotation:-(H.maxRadius*.5) });

    }// init_circles()

    function circle_distribution($elems, maxRadius, functions) {
        if(typeof functions === 'undefined') functions = {};
        var $parent = $elems.parent();

        var radius = $parent.width() *.5;
        //console.log('distribute radius', radius);
        var angleShift = convert_degrees(H.maxRadius*.5, 'deg-rad'); // to fit the circles' rotation we have to shift the angle
        var angle = 0 - angleShift;
        //var $elems = $parent.find(elem);
        var pi = Math.PI;
        var n = $elems.length;
        var circle_distance = pi*2 / n;
        
        if(typeof maxRadius !== 'undefined') {
            circle_distance *= convert_degrees(maxRadius, 'deg-pct'); // distribute distances to maxRadius
            circle_distance += circle_distance/(n-1);// make sure last point is at the end of maxRadius
        }


        $elems.each(function(i) {
            ////console.log('distribute angle', convert_degrees((angle+angleShift), 'rad-deg'), 'distr pct', convert_degrees(convert_degrees((angle+convert_degrees(H.maxRadius*.5, 'deg-rad')), 'rad-deg'), 'deg-pct'));
            var $thisElem = jQuery(this);
            var posX = radius + radius * Math.cos(angle);
            var posY = radius + radius * Math.sin(angle);
            var pctX = get_pctPos_from_pxPos(posX, (radius *2));
            var pctY = get_pctPos_from_pxPos(posY, (radius *2));

            $thisElem.css({
                'left' : pctX + '%',
                'top' : pctY + '%'
            });

            if(_.isFunction(functions.onElem)) functions.onElem($thisElem, {
                angle:{
                    radians: (angle + angleShift),
                    degrees: convert_degrees((angle+angleShift), 'rad-deg'),// angle starts from -maxRadius -> have to add it here to count from 0
                    percent: convert_degrees(convert_degrees((angle+angleShift), 'rad-deg'), 'deg-pct')
                },
                position: {
                    x:posX,
                    y:posY,
                    pctX:pctX,
                    pctY:pctY
                }
            });

            angle += circle_distance;
        });
    }// circle_distribution()

    function position_label($chapter, meta) {
        ////console.log(meta);
        $chapter.attr({
           'data-pct' : (meta.angle.percent*100).toFixed(2)
        });

        var is_left = meta.position.pctX - 50 < 0,
            is_top = meta.position.pctY - 50 < 0;
        $chapter.find('.chapter-label').css((is_top ? 'bottom' : 'top'), 5);
        $chapter.find('.chapter-label').css((is_left ? 'right' : 'left'), 15);
    }

    function convert_degrees(value, operation) {
        if(typeof operation === 'undefined') operation = 'pct-deg';

        var converted;

        switch(operation) {

            case 'pct-deg':
                if(value > 1) value *= .01;
                converted = 360 * value;
                break;
            case 'deg-pct' :
                converted = value / 360;
                break;
            case 'rad-deg':
                converted = value * (180 / Math.PI);
                break;
            case 'deg-rad' :
                converted = value * (Math.PI / 180);
                break;

        }// endswitch

        return converted;
    }// convert_degrees()

    function get_pctPos_from_pxPos(pos, parentsize) {
        return pos / (parentsize *.01);
    }

};// History()