import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from '../_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';


@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

  @Input() photos: Photo[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();

  baseUrl = environment.apiUrl;


   uploader: FileUploader;
   // tslint:disable-next-line:no-inferrable-types
   hasBaseDropZoneOver: boolean = false;

  constructor(private authService: AuthService, private userService: UserService, private aletify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {

    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);

        if (photo.isMain) {
          this.authService.changMemberPhoto(photo.url);
          this.authService.currentUser.photoUrl = photo.url;
          localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
        }
      }
    };

  }

  setMainPhoto(photo: Photo) {
    this.userService.setMainPhoto(this.authService.decodedToken.nameid, photo.id).subscribe(() => {
      this.photos.filter(p => p.isMain === true)[0].isMain = false;
      photo.isMain = true;
      this.getMemberPhotoChange.emit(photo.url);

      this.authService.changMemberPhoto(photo.url);
      this.authService.currentUser.photoUrl = photo.url;
      localStorage.setItem('user', JSON.stringify(this.authService.currentUser));


    }, error => {
      this.aletify.error(error);
    });
  }

  deletePhoto(id: number) {
    this.userService.deletePhoto(this.authService.decodedToken.nameid, id).subscribe(
      () => {
        this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
        this.aletify.success('Photo has been deleted');

      }, error => {
        this.aletify.error('Failed to delete the photo');
      }
    );
  }

}
