import * as React from 'react'
import { PercentCrop } from 'react-image-crop'
import { Image } from '../image/image'
import { ImageCrop } from '../imageCrop/imageCrop'

interface CroppableImageProps {
    isEditState: boolean,
    initImgSrc: string,
    setImage(image: File): void,
    crop: PercentCrop,
    setCrop(crop?: PercentCrop): void,
}

export const CroppableImage = ({isEditState, initImgSrc, setImage, crop, setCrop}: CroppableImageProps) => {

    const [imgSrc, setImgSrc] = React.useState("")
    const [isCropState, setIsCropState] = React.useState(false)

    const saveCrop = (crop: PercentCrop) => {
        setCrop(crop)
        setIsCropState(false)
    }

    const saveImage = (image: File) => {
        setImage(image)
        setCrop()
    }

    React.useEffect(() => {
        setImgSrc(initImgSrc)
    }, [initImgSrc])

    return isEditState && isCropState ?
    <ImageCrop imgSrc={imgSrc} initCrop={crop} saveCrop={saveCrop}/>
    :
    <Image imgSrc={imgSrc} setImgSrc={setImgSrc} isEditState={isEditState} setIsCropState={setIsCropState} saveImage={saveImage}/>
}