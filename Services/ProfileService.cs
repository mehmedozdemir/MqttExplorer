using Newtonsoft.Json;
using RabbitMQExplorer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RabbitMQExplorer.Services
{
    public class ProfileService
    {
        private readonly string _profilesPath;
        private List<ConnectionProfile> _profiles;

        public ProfileService()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "RabbitMQExplorer");
            
            Directory.CreateDirectory(appDataPath);
            _profilesPath = Path.Combine(appDataPath, "profiles.json");
            _profiles = LoadProfiles();
        }

        private List<ConnectionProfile> LoadProfiles()
        {
            try
            {
                if (File.Exists(_profilesPath))
                {
                    var json = File.ReadAllText(_profilesPath);
                    return JsonConvert.DeserializeObject<List<ConnectionProfile>>(json) ?? new List<ConnectionProfile>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Profiller yüklenemedi: {ex.Message}");
            }

            return new List<ConnectionProfile>();
        }

        private void SaveProfiles()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_profiles, Formatting.Indented);
                File.WriteAllText(_profilesPath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Profiller kaydedilemedi: {ex.Message}", ex);
            }
        }

        public List<ConnectionProfile> GetAllProfiles()
        {
            return new List<ConnectionProfile>(_profiles);
        }

        public ConnectionProfile? GetProfile(string id)
        {
            return _profiles.FirstOrDefault(p => p.Id == id);
        }

        public ConnectionProfile? GetDefaultProfile()
        {
            return _profiles.FirstOrDefault(p => p.IsDefault);
        }

        public void SaveProfile(ConnectionProfile profile)
        {
            var existing = _profiles.FirstOrDefault(p => p.Id == profile.Id);
            if (existing != null)
            {
                _profiles.Remove(existing);
            }

            if (profile.IsDefault)
            {
                // Clear other defaults
                foreach (var p in _profiles)
                {
                    p.IsDefault = false;
                }
            }

            _profiles.Add(profile);
            SaveProfiles();
        }

        public void DeleteProfile(string id)
        {
            var profile = _profiles.FirstOrDefault(p => p.Id == id);
            if (profile != null)
            {
                _profiles.Remove(profile);
                SaveProfiles();
            }
        }

        public void SetDefaultProfile(string id)
        {
            foreach (var p in _profiles)
            {
                p.IsDefault = (p.Id == id);
            }
            SaveProfiles();
        }
    }
}
