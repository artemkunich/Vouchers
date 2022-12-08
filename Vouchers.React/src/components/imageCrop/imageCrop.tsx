import * as React from 'react'
import ReactCrop, { PercentCrop } from 'react-image-crop'

import 'react-image-crop/dist/ReactCrop.css'
import 'react-image-crop/src/ReactCrop.scss'

const maxImageSide = 900;

interface ImageCropProps {
    imgSrc: string,
    initCrop: PercentCrop,
    saveCrop(crop: PercentCrop): void
}

export const ImageCrop = ({imgSrc, initCrop, saveCrop}: ImageCropProps) => {

    const [crop, setCrop] = React.useState(initCrop)

    const onImageLoad = async (e: React.FormEvent<HTMLImageElement>) => {

        if(crop)
            return

        const { width, height } = e.currentTarget

        const side = Math.min(width, height)

        const cropRelation = 66

        const percentWidth = cropRelation * side / width
        const percentHeight = cropRelation * side / height

        const percentX = (100 - percentWidth) / 2
        const percentY = (100 - percentHeight) / 2

        setCrop({
            unit: '%', // Can be 'px' or '%'
            x: percentX,
            y: percentY,
            //keepSelection : true,
            width: percentWidth,
            height: percentHeight,
        })
    }

    return <>
        <div className="row mb-2">
            <div className="col-12">
                <ReactCrop crop={crop} onChange={(pixelCrop, percentCrop) => setCrop(percentCrop)} aspect={1}>
                    <img id='croppedImage' style={{maxHeight: maxImageSide, maxWidth: maxImageSide}} src={imgSrc} onLoad={onImageLoad} />
                </ReactCrop>
            </div>
        </div>
        <div className="row mb-2">
            <div className="d-grid col-3">
                <button className="btn btn-primary" onClick={() => saveCrop(crop)}>Save</button>
            </div>
        </div>
    </>
}