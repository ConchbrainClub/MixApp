window.initPageSoftware = (dotnet) => {
    let scrollBox = document.querySelector('.page')

    // 内存泄漏 ！！！！！！！！！
    // dispose dotnet
    scrollBox.onscrollend = () => {
        let pageScrollHeight = scrollBox.scrollTop + scrollBox.clientHeight
        let pageHeight = document.querySelector('.page > .content').clientHeight

        let scrollEnd = (pageHeight - pageScrollHeight) < 500
        dotnet.invokeMethodAsync('OnScrollEnd', scrollEnd)
    }
}

window.locale = () => {
    return localStorage.getItem('locale') || window.navigator.language
}

window.initHighLight = (card) => {

    card.addEventListener('mouseover', function () {
        var light = this.querySelector('.light');
        light.style.display = 'block';

        this.addEventListener('mousemove', function (e) {
            var x = e.pageX - this.offsetLeft - light.offsetWidth / 2;
            var y = e.pageY + document.querySelector('.page').scrollTop - this.offsetTop - light.offsetHeight / 2;
            light.style.left = x + 'px';
            light.style.top = y + 'px';
        })
    })

    card.addEventListener('mouseout', function () {
        var light = this.querySelector('.light');
        light.style.display = 'none';
    })
}

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    let arrayBuffer = await contentStreamReference.arrayBuffer();
    let blob = new Blob([arrayBuffer]);
    let url = URL.createObjectURL(blob);
    let ele = document.createElement('a');
    ele.href = url;
    ele.download = fileName ?? '';
    ele.click();
    ele.remove();
    URL.revokeObjectURL(url);
}

window.reload = () => {
    location.reload()
}
