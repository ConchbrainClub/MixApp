try {
    window.external.receiveMessage(message => alert(message))
    console.log('client app')
} 
catch (error) {
    console.log('web app')
}

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
        var light = this.querySelector('.light')
        light.style.display = 'block'

        this.addEventListener('mousemove', function (e) {
            var x = e.pageX - this.offsetLeft - light.offsetWidth / 2
            var y = e.pageY + document.querySelector('.page').scrollTop - this.offsetTop - light.offsetHeight / 2
            light.style.left = x + 'px'
            light.style.top = y + 'px'
        })
    })

    card.addEventListener('mouseout', function () {
        var light = this.querySelector('.light')
        light.style.display = 'none'
    })
}

window.downloadQueue = []

window.downloadFile = (dotnet, fileName, url, cancelId) => {
    new Promise(async () => {

        let controller = new AbortController()
        let res = await fetch(url, {
            signal: controller.signal
        })

        downloadQueue.push({
            cancelId: cancelId,
            controller: controller
        })

        let reader = res.body.getReader()

        let contentLength = res.headers.get('Content-Length')
        let receivedLength = 0
        let buffer = []

        while (true) {
            let { done, value } = await reader.read()
            if (done) {
                dotnet.invokeMethodAsync('ChangedProgress', 100)
                break
            }

            buffer.push(value)
            receivedLength += value.length

            let progress = Math.round(receivedLength / contentLength * 100)
            if (isNaN(progress) || !isFinite(progress)) progress = 0

            dotnet.invokeMethodAsync('ChangedProgress', progress)
        }

        downloadQueue.splice(downloadQueue.findIndex(i => i.cancelId == cancelId), 1)
        
        let blob = new Blob(buffer)
        let ele = document.createElement('a')
        ele.href = URL.createObjectURL(blob)
        ele.download = fileName ?? ''
        ele.click()
        ele.remove()
        URL.revokeObjectURL(url)

    }).catch(() => {
        dotnet.invokeMethodAsync('ChangedProgress', -1)
    })
}

window.cancelDownloading = (cancelId) => {
    let index = downloadQueue.findIndex(i => i.cancelId == cancelId)
    if (index < 0) return

    try {
        downloadQueue[index].controller.abort()
        downloadQueue.splice(index, 1)
    } catch (error) {}
}

window.reload = () => {
    location.reload()
}