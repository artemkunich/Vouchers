import * as React from 'react'
import { noImageFoundSrc } from '../../api/imageSources';

const maxImageSide = 900;

interface ImageProps {
    imgSrc: string,
    setImgSrc(imgSrc: string): void,
    isEditState: boolean,
    setIsCropState(isCropState: boolean): void,
    saveImage(image: File): void

}

export const Image = ({imgSrc, setImgSrc, isEditState, setIsCropState, saveImage}: ImageProps) => {

    const onSelectFile = async (e: React.FormEvent<HTMLInputElement>) => {
        if(!e)
            return

        const target = e.target as HTMLInputElement

        if(!target || !target.files)
            return

        if (target.files.length > 0) {
            saveImage(target.files[0])
            const reader = new FileReader()
            reader.addEventListener('load', () => {
                setImgSrc(reader.result?.toString() || '')
                setIsCropState(true)
            })
            reader.readAsDataURL(target.files[0])
        }
    }

    return <>
        <div className="row mb-2">
            <div className="col-6">
                <img style={{maxHeight: maxImageSide, maxWidth: maxImageSide}} src={`${imgSrc ?? noImageFoundSrc}`}></img>
            </div>
        </div>
        { 
            isEditState &&
            <div className="row mb-2">                          
                <div className="d-grid col-3">
                    { imgSrc && <button className="btn btn-primary" onClick={() => {setIsCropState(true)}}>Crop image</button> }
                </div>
                <div className="col-3">
                    <input type="file" name="Image" onChange={onSelectFile}/>
                </div>
            </div>
        }
    </>
}