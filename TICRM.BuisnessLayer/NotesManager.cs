using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TICRM.BuisnessLayer.Base;
using TICRM.DTOs;
using TICRM.DAL;
namespace TICRM.BuisnessLayer
{
    public class NotesManager : BaseManager
    {
        public NotesDto GetNote(Guid? guid)
        {
            try
            {
                InsertEventLog("GetNote", EventType.Log, EventColor.yellow, "Successfully Enter in GetNote to Get Data on id", "TICRM.BusinessLayer.NoteManager", "");
                return objMapper.GetNoteDto(dbEnt.Notes.Find(guid)); // get activity on id and and convert it DTO and then return in response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetNote", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.NoteManager", "");
                throw ex;
            }
        }

        public List<NotesDto> GetCaseNotes(Guid? guid)
        {
            try
            {
                InsertEventLog("GetCaseNotes", EventType.Log, EventColor.yellow, "Successfully Enter to Get Data on Id", "TICRM.BusinessLayer.NoteManager.GetCaseNotes", "");
                List<NotesDto> notesDto = new List<NotesDto>(); // create list Object of Activity DTo

                List<DAL.Note> notes = dbEnt.Notes.Where(x => x.RelatedTo == RelatedToEnum.Cases.ToString() && x.RelatedToId == guid).ToList(); // Get List Of Activities from DB
                // apply iteration on activities
                foreach (DAL.Note item in notes.CollectionNotNull())
                {
                    notesDto.Add(objMapper.GetNoteDto(item)); // add in a list object
                }
                return notesDto; // return List Object in Response
            }
            catch (Exception ex)
            {
                InsertEventMonitor("GetCaseNotes", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRM.BusinessLayer.NoteManager.GetCaseNotes", "");
                throw;
            }
        }

        public bool SaveNote(NotesDto notesDto)
        {
            try
            {
                InsertEventLog("SaveNote", EventType.Log, EventColor.yellow, "Successfully Enter in SaveNote", "TICRM.BusinessLayer.NoteManager", "");

                Note notes; // create a new object
                notes = objMapper.GetNote(notesDto); // pass parameter object to activity object
                
                    InsertEventLog("SaveNote", EventType.Log, EventColor.yellow, "For Create Successfully Enter", "TICRMTICRM.BuisnessLayer.NoteManager.SaveNote", "");


                notes.NoteId = Guid.NewGuid();
                    notes.Note1= notesDto.Note1;
                    notes.CreatedDate = DateTime.Now;
                notes.RelatedTo = notesDto.RelatedTo;
                notes.RelatedToId = notesDto.RelatedToId;
                dbEnt.Notes.Add(notes); // add in a database
                    if (dbEnt.SaveChanges() > 0)
                    {
                        return true;
                    }
                



            }
            catch (Exception ex)
            {
                InsertEventMonitor("SaveNote", EventType.Exception, EventColor.red, ex.Message + " /n " + ex.StackTrace, "TICRMTICRM.BuisnessLayer.NoteManager.SaveNote", "");
                throw ex;
            }
            return false;

        }



    }
}
