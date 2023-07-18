
window.PageScrollHeight = () => {
    let scrollBox = document.querySelector('.page')
    return scrollBox.scrollTop + scrollBox.clientHeight
}

window.PageHeight = () => document.querySelector('.page > div').clientHeight
