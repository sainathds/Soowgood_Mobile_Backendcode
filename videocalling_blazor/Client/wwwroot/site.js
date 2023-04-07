let _videoTrack = null;
let _activeRoom = null;
let _participants = new Map();
let _dominantSpeaker = null;

async function getVideoDevices() {
    try {
        let devices = await navigator.mediaDevices.enumerateDevices();
        if (devices.every(d => !d.label)) {
            await navigator.mediaDevices.getUserMedia({
                video: true
            });
        }

        devices = await navigator.mediaDevices.enumerateDevices();
        if (devices && devices.length) {
            const deviceResults = [];
            devices.filter(device => device.kind === 'videoinput')
                .forEach(device => {
                    const { deviceId, label } = device;
                    deviceResults.push({ deviceId, label });
                });

            return deviceResults;
        }
    } catch (error) {
        alertify.error(`Unable to connect to Call: ${error.message}`);
    }

    return [];
}

async function startVideo(deviceId, selector) {
    
    const cameraContainer = document.querySelector(selector);
    if (!cameraContainer) {
        return;
    }

    try {
        if (_videoTrack) {
            _videoTrack.detach().forEach(element => element.remove());
        }

        _videoTrack = await Twilio.Video.createLocalVideoTrack({ deviceId });
        const videoEl = _videoTrack.attach();
        videoEl.style.transform = 'scale(-1, 1)';
        cameraContainer.append(videoEl);
    } catch (error) {
        alertify.error(`Unable to connect to Call: ${error.message}`);
    }
}

async function joinroomsuccess() {     
    $("#participants").find("div:eq(0)").removeClass('initiatecall');
    $("#participants").find("div:eq(0)").addClass('startedcall');
    return true;
}


async function unmuteandmutevideo(roomName, cammerastatus) {
  
    try {
        if (!_activeRoom || !_activeRoom.localParticipant)
            throw new Error('You must be connected to a room to mute tracks.');
        if (cammerastatus == true) {
            _activeRoom.localParticipant.videoTracks.forEach(
                publication => {
                    publication.track.disable();                   
                }               
            );
            return true;
        } else {
            _activeRoom.localParticipant.videoTracks.forEach(
                publication => {
                    publication.track.enable();
                  
                }
            );
            return true;
        }        
    } catch (error) {
        alertify.error(`Unable to connect to Call: ${error.message}`);
    }
}

async function unmuteandmuteaudio(roomName, cammerastatus) {
    try {
        if (!_activeRoom || !_activeRoom.localParticipant)
            throw new Error('You must be connected to a room to mute tracks.');
        if (cammerastatus == true) {
            _activeRoom.localParticipant.audioTracks.forEach(
                publication => publication.track.disable()
            );
        } else {
            _activeRoom.localParticipant.audioTracks.forEach(
                publication => publication.track.enable()
            );
        }
        return true;
    } catch (error) {
        alertify.error(`Unable to connect to Call: ${error.message}`);
    }
}

//function handleTrackDisabled(track) {
//    track.on('disabled', () => {
//        console.log('Nitin')
//    });
//}

async function createOrJoinRoom(roomName, token, serviceforname,username) {   
    try {
        if (_activeRoom) {
            _activeRoom.disconnect();
        }

        const audioTrack = await Twilio.Video.createLocalAudioTrack();
        const tracks = [audioTrack, _videoTrack];
        _activeRoom = await Twilio.Video.connect(
            token, {
            name: roomName,
            tracks,
            dominantSpeaker: true
        });

        if (_activeRoom) {
            initialize(_activeRoom.participants, serviceforname, username);
            _activeRoom
                .on('disconnected',
                    room => room.localParticipant.tracks.forEach(
                        publication => detachTrack(publication.track)))
                .on('participantConnected', participant => add(participant, serviceforname, username))
                .on('participantDisconnected', participant => remove(participant))
                .on('dominantSpeakerChanged', dominantSpeaker => loudest(dominantSpeaker));
        }
    } catch (error) {
        alertify.error(`Unable to connect to Call: ${error.message}`);
    }

    return !!_activeRoom;
}

function initialize(participants, serviceforname, username) {
    _participants = participants;
    if (_participants) {
        _participants.forEach(participant => registerParticipantEvents(participant, serviceforname, username));
    }
}

function add(participant, serviceforname, username) {
    if (_participants && participant) {
        _participants.set(participant.sid, participant);
        registerParticipantEvents(participant, serviceforname, username);
    }
}

function remove(participant) {
    if (_participants && _participants.has(participant.sid)) {
        _participants.delete(participant.sid);
    }
}

function loudest(participant) {
    _dominantSpeaker = participant;
}

function registerParticipantEvents(participant, serviceforname, username) {    
    if (participant) {
        participant.tracks.forEach(publication => subscribe(publication, serviceforname, username));
        participant.on('trackDisabled', publication => { dotrackDisabled(publication) });
        participant.on('trackEnabled', publication => { dotrackEnabled(publication) });
        participant.on('trackPublished', publication => subscribe(publication, serviceforname, username));
        participant.on('trackUnpublished',
            publication => {
                if (publication && publication.track) {
                    detachRemoteTrack(publication.track);
                }
            });
    }
}


function dotrackDisabled(publication) {
    if (publication.track.kind.toLowerCase() == "video") {
        $("video#" + publication.track.sid).removeClass("show");
        $("video#" + publication.track.sid).addClass("hide");
        $(".avtar#" + publication.track.sid).addClass("show");
        $("avtar#" + publication.track.sid).removeClass("hide");
        $(".remotecammera").removeClass('fa-video')
        $(".remotecammera").addClass('fa-video-slash')
    } else {
        $(".remotemic").removeClass('fa-microphone')
        $(".remotemic").addClass('fa-microphone-slash')
    }
}

function dotrackEnabled(publication) {
    if (publication.track.kind.toLowerCase() == "video") {
        $("video#" + publication.track.sid).addClass("show");
        $("video#" + publication.track.sid).removeClass("hide");
        $(".avtar#" + publication.track.sid).removeClass("show");
        $("avtar#" + publication.track.sid).addClass("hide");
        $(".remotecammera").addClass('fa-video')
        $(".remotecammera").removeClass('fa-video-slash')
    } else {
        $(".remotemic").addClass('fa-microphone')
        $(".remotemic").removeClass('fa-microphone-slash')
    }
}

function subscribe(publication, serviceforname, username) {    
    if (isMemberDefined(publication, 'on')) {
        publication.on('subscribed', track => {
            attachTrack(track, serviceforname, username);            
        }
           );
        publication.on('unsubscribed', track => detachTrack(track));
    }
}

function attachTrack(track, serviceforname, username) {
   
    if (isMemberDefined(track, 'attach')) {
        const audioOrVideo = track.attach();
        audioOrVideo.id = track.sid;

        if ('video' === audioOrVideo.tagName.toLowerCase()) {
            audioOrVideo.style.transform = 'scale(-1, 1)';
            if (_activeRoom.participants.size > 0) {
                $(".beforejoin").parent().hide();
            } else {
                $(".beforejoin").parent().show();
            }
            const responsiveDiv = document.createElement('div');
            responsiveDiv.id = track.sid;
            responsiveDiv.classList.add('embed-responsive');
            responsiveDiv.classList.add('embed-responsive-16by9');

            const responsiveItem = document.createElement('div');
            responsiveItem.classList.add('embed-responsive-item');
           // responsiveItem.classList.add('embed-responsive-participant');

            // Similar to.
            // <div class="embed-responsive embed-responsive-16by9">
            //   <div id="camera" class="embed-responsive-item">
            //     <video></video>
            //   </div>
            // </div>
            responsiveItem.appendChild(audioOrVideo);

            const responsiveItem1 = document.createElement('div');
            responsiveItem1.id = track.sid;
            responsiveItem1.classList.add('row');
            responsiveItem1.classList.add('avtar');
            responsiveItem1.classList.add('hide');

            const responsiveItem2 = document.createElement('div');     
            responsiveItem2.classList.add('col-md-8');
            responsiveItem2.classList.add('col-sm-12');
            responsiveItem2.classList.add('col-xs-12');
            responsiveItem2.classList.add('offset-md-2');
            responsiveItem2.classList.add('inneravtar');
            const responsiveItem3 = document.createElement('div');            
            responsiveItem3.textContent = serviceforname;
            responsiveItem3.classList.add('innertext');
            responsiveItem2.appendChild(responsiveItem3);
            responsiveItem1.appendChild(responsiveItem2);
            responsiveItem.appendChild(responsiveItem1);
            

            const responsiveItem4 = document.createElement('div');
            responsiveItem4.classList.add('remote');
            responsiveItem4.classList.add('col-12');

            const responsiveItem5 = document.createElement('i');
            responsiveItem5.classList.add('remotemic');
            responsiveItem5.classList.add('fas');
            responsiveItem5.classList.add('fa-microphone');

            const responsiveItem6 = document.createElement('i');
            responsiveItem6.classList.add('remotecammera');
            responsiveItem6.classList.add('fas');
            responsiveItem6.classList.add('fa-video');

            responsiveItem4.appendChild(responsiveItem5);
            responsiveItem4.appendChild(responsiveItem6);
            responsiveItem.appendChild(responsiveItem4);
            responsiveDiv.appendChild(responsiveItem);
            document.getElementById('participants').appendChild(responsiveDiv);

            //var elementusername = document.getElementsByClassName('username');
            //if (typeof (elementusername) != 'undefined' && elementusername != null) {
            //    if (elementusername.length != 0) {
            //        document.getElementById('participants').removeChild(elementusername)
            //    }
               
            //} 
            //const responsiveItem4 = document.createElement('div');
            //responsiveItem4.classList.add('col-12');
            //responsiveItem4.classList.add('username');
            //responsiveItem4.textContent = username;             
            //document.getElementById('participants').appendChild(responsiveItem4);

            
        } else {
            document.getElementById('participants')
                .appendChild(audioOrVideo);
        }
    }
}




function detachTrack(track) {
    if (this.isMemberDefined(track, 'detach')) {
        track.detach()
            .forEach(el => {
                if ('video' === el.tagName.toLowerCase()) {
                    const parent = el.parentElement;
                    if (parent && parent.id !== 'camera') {
                        const grandParent = parent.parentElement;
                        if (grandParent) {
                            grandParent.remove();
                        }
                    }
                    alertify.error("Participant leave the call.")
                } else {
                    el.remove()
                }
            });

        $(".beforejoin").parent().show();
       
    }
}

function isMemberDefined(instance, member) {
    return !!instance && instance[member] !== undefined;
}

//async function alertyfyconfirm(message, onsuccess) {
//    alertify.confirm(message,
//        function () {
//            onsuccess(true);
//        }, function () {
//            onsuccess(false);
//    }).set({ "title": "Information" }).set('labels', { ok: 'Yes', cancel: 'No' });
//}

async function leaveRoom() {
    try {
        if (_activeRoom) {
            _activeRoom.localParticipant.tracks.forEach(publication => {
                publication.track.stop();
                const attachedElements = publication.track.detach();
                console.log("unsubscribed from: " + publication.track)
                attachedElements.forEach(element => element.remove());
            });
            _activeRoom.disconnect();            
            _activeRoom = null;
        }

        if (_participants) {
            _participants.clear();
        }
    }
    catch (error) {
        alertify.error(`Unable to connect to Call: ${error.message}`);
    }
}





window.videoInterop = {
    getVideoDevices,
    startVideo,
    createOrJoinRoom,
    leaveRoom,
    joinroomsuccess,
    unmuteandmutevideo,
    unmuteandmuteaudio
   // alertyfyconfirm
};