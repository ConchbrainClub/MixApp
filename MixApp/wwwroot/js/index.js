window.InitPageSoftware = (dotnet) => {
    let scrollBox = document.querySelector('.page')

    scrollBox.onscrollend = () => {
        let pageScrollHeight = scrollBox.scrollTop + scrollBox.clientHeight
        let pageHeight = document.querySelector('.page > .content').clientHeight

        let scrollEnd = (pageHeight - pageScrollHeight) < 400
        dotnet.invokeMethodAsync('OnScrollEnd', scrollEnd)
    }
}