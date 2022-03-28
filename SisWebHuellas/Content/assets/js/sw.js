//var cacheName = 'v7';

//// Default files to always cache
//var cacheFiles = [
//    '/',
//    '/Web.config'
//    //'/SisWebHuellas/',
//    //'/SisWebHuellas/Web.config',
//    //'/SisWebHuellas/Global.asax',
//    //'/SisWebHuellas/favicon.ico',
//    //'/SisWebHuellas/ApplicationInsights.config',
//    ////HTML
//    //'/SisWebHuellas/Views/Login/Login.cshtml',
//    //'/SisWebHuellas/Views/Login/ListarLogin.cshtml',
//    //'/SisWebHuellas/Views/Login/_Login.cshtml',
//    ////JS
//    //'/SisWebHuellas/Content/assets/js/lgnew.js',
//    //'/SisWebHuellas/Content/vendors/jquery/dist/jquery.min.js',
//    ////CSS
//    //'/SisWebHuellas/Content/login/css/sb-admin-2.min.css'

//    //'./',
//    //'./index.html',
//    //'./favicon-96x96.png',
//    //'./favicon-196x196.png',
//    //'https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,900'
//]


self.addEventListener('install', function (event) {

    console.log('[ServiceWorker] Installed');
    //console.log(cacheFiles);
    // console.log(cacheName);

    console.log(event);
    event.waitUntil(

        caches.open('v1').then(function (cache) {
            return cache.add('./SisWebHuellas/Web.config');
        })

            //    caches.open('v1').then(function (cache) {
            //        console.log('[ServiceWorker] Caching cacheFiles');
            //        return cache.addAll(//cacheFiles
            //            [
            //            '/images/_logo.png',
            //            '/images/logo.png'
            //            //[
            //            ////'/sw-test/',
            //            ////'/sw-test/index.html',
            //            ////'/sw-test/style.css',
            //            ////'/sw-test/app.js',
            //            ////'/sw-test/image-list.js',
            //            ////'/sw-test/star-wars-logo.jpg',
            //            ////'/sw-test/gallery/bountyHunters.jpg',
            //            ////'/sw-test/gallery/myLittleVader.jpg',
            //            ////'/sw-test/gallery/snowTroopers.jpg'
            //            ]
            //        );
            //    })
    );// end e.waitUntil
});

self.addEventListener('activate', function (event) {
    console.log('[ServiceWorker] Activated');

    event.waitUntil(

        // Get all the cache keys (cacheName)
        caches.keys().then(function (cacheNames) {
            return Promise.all(cacheNames.map(function (thisCacheName) {

                // If a cached item is saved under a previous cacheName
                if (thisCacheName !== 'v1') {

                    // Delete that cached file
                    console.log('[ServiceWorker] Removing Cached Files from Cache - ', thisCacheName);
                    return caches.delete(thisCacheName);
                }
            }));
        })
    ); // end e.waitUntil

});

//self.addEventListener('fetch', function (e) {
//    console.log('[ServiceWorker] Fetch', e.request.url);

//    // e.respondWidth Responds to the fetch event
//    e.respondWith(

//        // Check in cache for the request being made
//        caches.match(e.request)


//            .then(function (response) {

//                // If the request is in the cache
//                if (response) {
//                    console.log("[ServiceWorker] Found in Cache", e.request.url, response);
//                    // Return the cached version
//                    return response;
//                }

//                // If the request is NOT in the cache, fetch and cache

//                var requestClone = e.request.clone();
//                fetch(requestClone)
//                    .then(function (response) {

//                        if (!response) {
//                            console.log("[ServiceWorker] No response from fetch ")
//                            return response;
//                        }

//                        var responseClone = response.clone();

//                        //  Open the cache
//                        caches.open('v1').then(function (cache) {

//                            // Put the fetched response in the cache
//                            cache.put(e.request, responseClone);
//                            console.log('[ServiceWorker] New Data Cached', e.request.url);

//                            // Return the response
//                            return response;

//                        }); // end caches.open

//                    })
//                    .catch(function (err) {
//                        console.log('[ServiceWorker] Error Fetching & Caching New Data', err);
//                    });


//            }) // end caches.match(e.request)
//    ); // end e.respondWith
//});


//self.addEventListener('fetch', event => {
//    console.log("Eventos del SW");
//    console.log(event);

//});
