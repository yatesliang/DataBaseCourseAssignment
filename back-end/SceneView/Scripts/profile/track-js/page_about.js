//console.log('about js', _history_data);

init_about();

function init_about() {

    BUNDY.$objects.contentBlockHistory = jQuery('.content-block.history');
    BUNDY.vars.historyChapters = _history_data;

    var chapterimgs = get_chapter_imgs(BUNDY.vars.historyChapters);
    //add_imgs_to_preload_stack(chapterimgs, BUNDY.images);
    BUNDY.instances.History = new History(BUNDY.$objects.contentBlockHistory, {
        chapters: BUNDY.vars.historyChapters,
        functions : {
            onInit: function(e) {
                console.log('initializing History JS at ', e);
                preload_images(chapterimgs);
            }
        }
    });

}// init_about()

function get_chapter_imgs(chapters) {
    var imgs = [];

    for(var i=0; i<chapters.length; i++) {
        imgs.push(chapters[i].img.src);
    }// endfor

    return imgs;
}//get_chapter_imgs()