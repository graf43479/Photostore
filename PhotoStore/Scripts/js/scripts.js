$(document).ready(function() {

	/* Монтаж блоков в портфолио */
	/*setTimeout(function() {
		$('#portfolio, #revs').wookmark({
			flexibleWidth: false,
			autoResize: false,
			offset: 0
		});
	}, 1000);*/
	
	$('#portfolio, #revs').freetile({
		animate: true,
		elementDelay: 30,
		selector: '.portfolio-modal, .rev'
	});
	
	/* Ресайз картинок при изменении размера окна */
	/*$(window).resize(function(){
		$('#portfolio, #revs').wookmark({
			flexibleWidth: false,
			autoResize: false,
			offset: 0
		});
	});*/
	
	/* Модалки */
	$('a[data-rel^=lightcase]').lightcase({
		swipe: true,
		showTitle: false,
		showCaption: false,
		showSequenceInfo: false,
		closeOnOverlayClick: false,
		transition: 'scrollHorizontal',
		maxWidth : 1200,
		maxHeight : 800,
	});	
	
	/* Контактная форма */
	//$('.submit_login').click(function(){
	//	$('#load').show();
	//	var name = $('#Name').val();
	//	var email = $('#Email').val();
	//	var mes = $('#Text').val();
		
	//	$.post(
	//	'/Home/Contact',
	//	{Name:name,Email:email,Text:mes},
	//	function(data){
	//		$('#result').html(data);
	//		$('#load').hide();
	//	});
		
	//	return false;
	//});

    

    
	

	


	/* Мобильное меню */
	$('.button-menu').click(function() {
		if($(this).hasClass('active')) {
			$('.mobile-menu').slideUp(500);
			$(this).removeClass('active');
		}
		else {
			$('.mobile-menu').slideDown(500);
			$(this).addClass('active');
		}
	});
});